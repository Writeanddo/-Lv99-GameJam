using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.CinemachineOrbitalTransposer;

public enum CilinderState
{
    Start,
    Holding,
    Stopped,
    Reverse,
    Waiting
}

public class OxygenSystem : MonoBehaviour
{
    private PlayerController _playerController;
    private Animator oxigenAn;

    [Header("Oxygen Cilinder")]
    [SerializeField] private float playerAnimationTime = 1.1f;
    [SerializeField] private float breathAnimation = 1f;
    [SerializeField] private float upAnimationTime = 1.1f;
    [SerializeField] private float divisor = 2;

    [SerializeField] private SpriteRenderer barOxigenSr;
    [SerializeField] private SpriteRenderer barOxigenSrBG;
    [SerializeField] private Transform animationPoint;
    [SerializeField] private AnimatorDetectGamepad gamepadUI;

    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private float cooldownTimer = 5f;

    private bool canInteractWithCilinder = true;
    private bool isOxygenStart;
    private CilinderState state = CilinderState.Start;

    private float cilinderOxygen = 0;

    private bool validCancel;
    private bool validHolding;

    private float timeRemaining = 0;

    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        oxigenAn = GetComponent<Animator>();

        barOxigenSr.gameObject.SetActive(false);
        barOxigenSrBG.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);

        state = CilinderState.Waiting;
    }

    private void Update()
    {
        if (state == CilinderState.Waiting)
        {
            if (timeRemaining >= 0)
            {
                timer.gameObject.SetActive(true);

                timeRemaining -= Time.deltaTime;

                TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
                string timeString = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);

                timer.text = timeString;
            }
            else if (timer.gameObject.activeSelf == true)
            {
                timer.gameObject.SetActive(false);
                canInteractWithCilinder = true;
            }
        }
    }

    public void StartCilinder()
    {
        if (canInteractWithCilinder && isOxygenStart == false)
        {
            StartCoroutine(Start_IEnumerator());
        }
    }

    private IEnumerator Start_IEnumerator()
    {
        if (state == CilinderState.Waiting)
        {
            ChangeState(CilinderState.Start);

            yield return new WaitForSecondsRealtime(playerAnimationTime);

            ChangeState(CilinderState.Stopped);  
        }
    }

    public void HoldCilinder()
    {
        if (validHolding && state == CilinderState.Stopped)
        {
            ChangeState(CilinderState.Holding);

            StartCoroutine(ReloadOxygenBar());
        }
    }

    private IEnumerator ReloadOxygenBar()
    {
        barOxigenSr.gameObject.SetActive(true);
        barOxigenSrBG.gameObject.SetActive(true);

        barOxigenSr.size = new(barOxigenSr.size.x, 0);
        barOxigenSrBG.size = new(barOxigenSrBG.size.x, 1);

        while (state == CilinderState.Holding)
        {
            for (int i = 0; i < 100; i++)
            {
                cilinderOxygen += 0.01f;
                barOxigenSr.size = new Vector2(0.56f, cilinderOxygen);
                yield return new WaitForSecondsRealtime(0.01f);

            }

            for (int i = 0; i < 100; i++)
            {
                cilinderOxygen -= 0.01f;
                barOxigenSr.size = new Vector2(0.56f, cilinderOxygen);
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
    }

    public void StopCilinder()
    {
        if (state == CilinderState.Holding)
        {
            StopCoroutine(ReloadOxygenBar());

            StartCoroutine(WaitForAnimation());

            barOxigenSr.size = new(barOxigenSr.size.x, 0);
            barOxigenSrBG.size = new(barOxigenSrBG.size.x, 0);

            
        }
    }

    private IEnumerator WaitForAnimation()
    {
        barOxigenSr.size = new Vector2(0.56f, cilinderOxygen);

        _playerController.PlayerOxygen.AddOxygen(cilinderOxygen / divisor);

        cilinderOxygen = 0;

        barOxigenSr.gameObject.SetActive(false);
        barOxigenSrBG.gameObject.SetActive(false);

        oxigenAn.SetTrigger("Breath");

        yield return new WaitForSecondsRealtime(breathAnimation);

        if (_playerController.PlayerOxygen.IsOnMaxOxygen())
        {
            Debug.Log("Max Oxygen");

            ChangeState(CilinderState.Reverse);

            yield return new WaitForSecondsRealtime(upAnimationTime);

            _playerController.transform.position = animationPoint.position;
            _playerController.ActivePlayer();

            GameManager.Instance.ResumeTemporaryPause();

            ChangeState(CilinderState.Waiting);
        }
        else
        {
            Debug.Log("Not Max Oxygen");

            ChangeState(CilinderState.Stopped);
        }
    }

    private void ChangeState(CilinderState state)
    {
        Debug.Log("New State: " + state);

        switch (state)
        {
            case CilinderState.Start:

                oxigenAn.SetTrigger("Start");

                canInteractWithCilinder = false;
                isOxygenStart = true;

                _playerController.ResetWalk();
                _playerController.DisablePlayer();
                GameManager.Instance.TemporaryPause();

                break;

            case CilinderState.Holding:

                validCancel = true;

                gamepadUI.Disable();

                break;

            case CilinderState.Stopped:

                oxigenAn.SetTrigger("Stop");

                gamepadUI.Active();

                validHolding = true;
                validCancel = false;

                break;

            case CilinderState.Reverse:

                oxigenAn.SetTrigger("Reverse");

                break;

            case CilinderState.Waiting:

                isOxygenStart = false;
                validCancel = false;
                validHolding = false;

                timeRemaining = cooldownTimer;

                barOxigenSr.gameObject.SetActive(false);
                barOxigenSrBG.gameObject.SetActive(false);

                break;
        }

        this.state = state;
    }

    public bool CanInteract()
    {
        return canInteractWithCilinder;
    }

    public bool ValidCancel()
    {
        return validCancel;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state != CilinderState.Waiting)
            return;

        if (canInteractWithCilinder == false)
            return;

        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.currentCilinder = this;
            _playerController = player;
            gamepadUI.Active();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (state != CilinderState.Waiting)
            return;

        if (isOxygenStart)
            return;

        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.currentCilinder = null;
            _playerController = null;
            gamepadUI.Disable();
        }
    }
}
