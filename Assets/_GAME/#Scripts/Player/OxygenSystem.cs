using System;
using System.Collections;
using TMPro;
using UnityEngine;

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

    [SerializeField] public SpriteRenderer barOxigenSr;
    //[SerializeField] private GameObject barOxigenSrBG;
    [SerializeField] private GameObject HUDbarOxigen;
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

        timer.gameObject.SetActive(false);
        HUDbarOxigen.SetActive(false);
        state = CilinderState.Waiting;
    }

    private void Update()
    {
        if(state == CilinderState.Waiting)
        {
            if (timeRemaining >= 0)
            {
                timer.gameObject.SetActive(true);

                timeRemaining -= Time.deltaTime;

                TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
                string timeString = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);

                timer.text = timeString;
            }
            else if(timer.gameObject.activeSelf == true)
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
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Combat", 0f);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/O2 Attach", GetComponent<Transform>().position);
            canInteractWithCilinder = false;
            isOxygenStart = true;

            StartCoroutine(Start_IEnumerator());
        }

    }

    private IEnumerator Start_IEnumerator()
    {
        if (state == CilinderState.Waiting)
        {
            state = CilinderState.Start;

            oxigenAn.SetTrigger("Start");

            _playerController.ResetWalk();
            _playerController.DisablePlayer();
            GameManager.Instance.TemporaryPause();

            yield return new WaitForSecondsRealtime(playerAnimationTime);

            oxigenAn.SetTrigger("Stop");

            state = CilinderState.Stopped;
            gamepadUI.Active();
            HUDbarOxigen.SetActive(true);

            validHolding = true;
        }
    }

    private IEnumerator ReloadOxygenBar()
    {
        gamepadUI.Disable();

        barOxigenSr.gameObject.SetActive(true);


        barOxigenSr.size = new(barOxigenSr.size.x, 0);
  

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

    public void HoldCilinder()
    {
        if (!validHolding)
            return;

        if (state == CilinderState.Stopped)
        {
            state = CilinderState.Holding;

            validCancel = true;

            StartCoroutine(ReloadOxygenBar());
        }
    }

    public void StopCilinder()
    {
        if (state == CilinderState.Holding)
        {
            state = CilinderState.Reverse;

            StopCoroutine(ReloadOxygenBar());

            StartCoroutine(WaitForAnimation());

            barOxigenSr.gameObject.SetActive(false);
            HUDbarOxigen.SetActive(false);

            barOxigenSr.size = new(barOxigenSr.size.x, 0);
   

            validCancel = false;
            validHolding = false;

            timeRemaining = cooldownTimer;
        }
    }

    private IEnumerator WaitForAnimation()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/O2 Breath", GetComponent<Transform>().position);

        barOxigenSr.size = new Vector2(0.56f, cilinderOxygen);

        _playerController.PlayerOxygen.AddOxygen(cilinderOxygen / divisor);

        oxigenAn.SetTrigger("Breath");

        yield return new WaitForSecondsRealtime(breathAnimation);

        oxigenAn.SetTrigger("Reverse");

        yield return new WaitForSecondsRealtime(upAnimationTime);

        _playerController.transform.position = animationPoint.position;
        _playerController.ActivePlayer();

        GameManager.Instance.ResumeTemporaryPause();

        isOxygenStart = false;

        state = CilinderState.Waiting;
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

        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.currentCilinder = null;
            _playerController = null;
            gamepadUI.Disable();
        }
    }
}
