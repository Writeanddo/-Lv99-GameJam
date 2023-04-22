using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPuzzle : Singleton<CheckPuzzle>
{
    public GameObject[] Puzzle1;
    public GameObject[] Puzzle2;
    public GameObject[] Puzzle3;


    private void Start()
    {
        Puzzle1[0].SetActive(true);
        Puzzle1[1].SetActive(false);

        Puzzle2[0].SetActive(true);
        Puzzle2[1].SetActive(false);

        Puzzle3[0].SetActive(true);
        Puzzle3[1].SetActive(false);


    }
}
