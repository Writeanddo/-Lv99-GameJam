using System.Collections;
using UnityEngine;

public class OxygenSystem : MonoBehaviour
{
    public PlayerController _PlayerController;
    private Animator oxigenAn;

    [Header("Oxygen Cilinder")]
    public bool isOxygenStart;
    public float cilinderOxygen;
    public SpriteRenderer barOxigenSr;

    // Start is called before the first frame update
    void Start()
    {
        _PlayerController = FindObjectOfType<PlayerController>();
        oxigenAn = GetComponent<Animator>();
    }

    public void StartCilinder()
    {
        if(isOxygenStart)
        {
            return;
        }

        oxigenAn.SetTrigger("start");

        if (isOxygenStart == false)
        {
            isOxygenStart = true;
            StartCoroutine(ReloadOxygenBar());
        }

        GameManager.Instance.TemporaryPause();
    }

    public void HoldCilinder()
    {

    }

    public void StopCilinder()
    {
        cilinderOxygen = 0;

        StopCoroutine(ReloadOxygenBar());

        isOxygenStart = false;
        cilinderOxygen = 0;
        barOxigenSr.size = new Vector2(0.56f, cilinderOxygen);

        GameManager.Instance.ResumeTemporaryPause();
    }

    private IEnumerator ReloadOxygenBar()
    {
        for (int i = 0; i < 100; i++)
        {
            if (isOxygenStart == true)
            {

                cilinderOxygen += 0.01f;
                print(cilinderOxygen);
                barOxigenSr.size = new Vector2(0.56f, cilinderOxygen);
                yield return new WaitForSeconds(0.01f);
            }
        }

        for (int i = 0; i < 100; i++)
        {
            if (isOxygenStart == true)
            {
                cilinderOxygen -= 0.01f;
                print(cilinderOxygen);
                barOxigenSr.size = new Vector2(0.56f, cilinderOxygen);
                yield return new WaitForSeconds(0.01f);
            }
        }

        if (isOxygenStart == true)
        {
            StartCoroutine(ReloadOxygenBar());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.oxygenCilinder = this;
        }
    }
}
