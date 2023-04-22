using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerPuzzle3 : MonoBehaviour
{

    private void OnEnable()
    {
        Puzzle3.Instance.StartCoroutine(Puzzle3.Instance.SpawnItemsRepeatedly());
    }
}
