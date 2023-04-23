using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] private float upAnimationTime = 1.1f;

    [SerializeField] private SpriteRenderer barOxigenSr;
    [SerializeField] private SpriteRenderer barOxigenSrBG;
    [SerializeField] private Transform animationPoint;
    [SerializeField] private AnimatorDetectGamepad gamepadUI;

    private bool canInteractWithCilinder = true;
    private bool isOxygenStart;
    private CilinderState state = CilinderState.Start;

    private float cilinderOxygen = 0;

    private bool validCancel;
    private bool validHolding;

    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        oxigenAn = GetComponent<Animator>();

        barOxigenSr.gameObject.SetActive(false);
        barOxigenSrBG.gameObject.SetActive(false);

        state = CilinderState.Waiting;
    }

    public void StartCilinder()
    {
        if (canInteractWithCilinder && isOxygenStart == false)
        {
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
            _playerController.DesativePlayer();
            GameManager.Instance.TemporaryPause();

            yield return new WaitForSecondsRealtime(playerAnimationTime);

            oxigenAn.SetTrigger("Stop");

            state = CilinderState.Stopped;
            gamepadUI.Active();

            validHolding = true;
        }
    }

    private IEnumerator ReloadOxygenBar()
    {
        gamepadUI.Disable();

        barOxigenSr.gameObject.SetActive(true);
        barOxigenSrBG.gameObject.SetActive(true);

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

            _playerController.PlayerOxygen.AddOxygen(cilinderOxygen);

            cilinderOxygen = 0;

            isOxygenStart = false;

            barOxigenSr.size = new Vector2(0.56f, cilinderOxygen);

            StopCoroutine(ReloadOxygenBar());

            StartCoroutine(WaitForAnimation());

            barOxigenSr.gameObject.SetActive(false);
            barOxigenSrBG.gameObject.SetActive(false);

            barOxigenSrBG.size = new(barOxigenSrBG.size.x, 0);

            validCancel = false;
            validHolding = false;
        }
    }

    private IEnumerator WaitForAnimation()
    {
        oxigenAn.SetTrigger("Reverse");

        yield return new WaitForSecondsRealtime(upAnimationTime);

        _playerController.transform.position = animationPoint.position;
        _playerController.ActivePlayer();

        GameManager.Instance.ResumeTemporaryPause();

        state = CilinderState.Waiting;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state != CilinderState.Waiting)
            return;

        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.currentCilinder = this;
            _playerController = player;
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
        }
    }

    public bool CanInteract()
    {
        return canInteractWithCilinder;
    }

    public bool ValidCancel()
    {
        return validCancel;
    }
}
