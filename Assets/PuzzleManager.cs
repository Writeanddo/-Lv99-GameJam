using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : Singleton<PuzzleManager>
{
    public List<GameObject> puzzleList = new List<GameObject>();
    public List<GameObject> pcPuzzle = new List<GameObject>();

    private void OnEnable()
    {
        ShufflePositionPuzzle();
    }

    private void ShufflePositionPuzzle()
    {
        for (int i = 0; i < pcPuzzle.Count; i++)
        {
            int randIndex = Random.Range(0, puzzleList.Count);
            var local = pcPuzzle[i].GetComponent<PuzzleLocalController>();
             local.Puzzlechoice = puzzleList[randIndex];
            puzzleList.RemoveAt(randIndex);
            
            
          



        }
    }

}
