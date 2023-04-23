using System.Collections;
using UnityEngine;

public enum CilinderState
{
    Start,
    Holding,
    Stopped
}

public class OxygenSystem : MonoBehaviour
{
    private PlayerController _playerController;
    private Animator oxigenAn;

    [Header("Oxygen Cilinder")]
    [SerializeField] private float playerAnimationTime = 2f;

    [SerializeField] private SpriteRenderer barOxigenSr;
    [SerializeField] private Transform animationPoint;
    [SerializeField] private AnimatorDetectGamepad gamepadUI;

    private bool canInteractWithCilinder = true;
    private bool isOxygenStart;
    private CilinderState state = CilinderState.Start;

    private float cilinderOxygen = 0;

    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        oxigenAn = GetComponent<Animator>();

        barOxigenSr.size = new Vector2(0.56f, 0f);
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
        state = CilinderState.Start;

        barOxigenSr.gameObject.SetActive(true);

        canInteractWithCilinder = false;
            isOxygenStart = true;

            oxigenAn.SetTrigger("Start");
    
            _playerController.DesativePlayer();

            GameManager.Instance.TemporaryPause();

            StartCoroutine(ReloadOxygenBar());

            yield return new WaitForSecondsRealtime(playerAnimationTime);

            state = CilinderState.Holding;

            gamepadUI.Active();
    }

    public void HoldCilinder()
    {
        if(state  == CilinderState.Holding)
        {
            gamepadUI.Disable();
        }   
    }

    public void StopCilinder()
    {
        if(state == CilinderState.Holding)
        {
            oxigenAn.SetTrigger("Stop");

            state = CilinderState.Stopped;

            _playerController.PlayerOxygen.AddOxygen(cilinderOxygen);

            cilinderOxygen = 0;

            isOxygenStart = false;
            
            barOxigenSr.size = new Vector2(0.56f, cilinderOxygen);
           

            StopCoroutine(ReloadOxygenBar());
            
            StartCoroutine(WaitForAnimation());

            barOxigenSr.gameObject.SetActive(false);
        }
    }

    private IEnumerator WaitForAnimation()
    {
        oxigenAn.SetTrigger("Reverse");

        yield return new WaitForSecondsRealtime(playerAnimationTime);

        GameManager.Instance.ResumeTemporaryPause();

        _playerController.transform.position = animationPoint.position;
        _playerController.ActivePlayer();
    }

    private IEnumerator ReloadOxygenBar()
    {
        if (isOxygenStart == true)
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

            StartCoroutine(ReloadOxygenBar());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.currentCilinder = this;
            _playerController = player;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.currentCilinder = null;
            _playerController = null;
        }
    }
}
