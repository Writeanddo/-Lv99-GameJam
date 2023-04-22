using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle2Controller : Singleton<Puzzle2Controller>
{
    public GameObject tutorial;
    public GameObject play;
    public GameObject win;


    public void ExitPuzzle()
    {
        var player = FindAnyObjectByType<PlayerController>();
        player.isPressedPuzzle = false;
        player.isPuzzleStart = false;
        GameManager.Instance.Puzzle1 = true;
        CheckPuzzle.Instance.Puzzle1[0].SetActive(false);
        CheckPuzzle.Instance.Puzzle1[1].SetActive(true);
        Destroy(gameObject);
    }

    public void StartPuzzler()
    {
        tutorial.SetActive(false);
        play.SetActive(true);
    }

    private void OnDisable()
    {
        tutorial.SetActive(true);
        play.SetActive(false);
        win.SetActive(false);
    }

}
