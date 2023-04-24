using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Puzzle3 : Singleton<Puzzle3>
{
    public GameObject[] itemPrefab;
    public RectTransform canvasRect;
    public RectTransform painelRect;
    public float spawnInterval = 5f;

    public int totalPoints = 0;
    public TextMeshProUGUI pointsUI;

    [Header("paineis")]
    public GameObject play;
    public GameObject tutorial;
    public GameObject win;
    private void Start()
    {
        pointsUI.text = totalPoints.ToString();
    }

    public IEnumerator SpawnItemsRepeatedly()
    {


        yield return new WaitForSecondsRealtime(spawnInterval);
        SpawnItem();

    }

    private void OnDisable()
    {
        tutorial.SetActive(true);
        play.SetActive(false);
        win.SetActive(false);

    }

    public void StarGame()
    {
        tutorial.SetActive(false);
        play.SetActive(true);
    }

    public void SetRestart()
    {
        play.SetActive(true);

    }

    public void SeExit()
    {
        win.SetActive(false);
        var player = FindAnyObjectByType<PlayerController>();
        player.isPressedPuzzle = false;
        player.isPuzzleStart = false;
        GameManager.Instance.Puzzle3 = true;
        CheckPuzzle.Instance.Puzzle3[0].SetActive(false);
        CheckPuzzle.Instance.Puzzle3[1].SetActive(true);

        GameManager.Instance.PuzzleComplete();

        Destroy(gameObject);
    }

    public void UpdatePoint()
    {
        pointsUI.text = totalPoints.ToString();
    }

    private void SpawnItem()
    {
        if (totalPoints <= 9)
        {
            int index = Random.Range(0, itemPrefab.Length);
            GameObject itemObj = Instantiate(itemPrefab[index], canvasRect.transform);
            float painelWidth = painelRect.rect.width;
            float painelHeight = painelRect.rect.height;

            float x = Random.Range(painelRect.anchoredPosition.x - painelWidth / 2, painelRect.anchoredPosition.x + painelWidth / 2);
            float y = Random.Range(painelRect.anchoredPosition.y - painelHeight / 2, painelRect.anchoredPosition.y + painelHeight / 2);

            itemObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            StartCoroutine(SpawnItemsRepeatedly());
        }
        else
        {
            win.SetActive(true);
            play.SetActive(false);
        }


    }
}
