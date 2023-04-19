using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsOrderPuzzle : MonoBehaviour
{
    public List<GameObject> listObjeto = new List<GameObject>();

    public int currentObjectIndex;

    void Start()
    {
        ShuffleObjects();
        currentObjectIndex = 0; 
    }

    void ShuffleObjects()
    {
        for (int i = 0; i < listObjeto.Count; i++)
        {
            int randIndex = Random.Range(i, listObjeto.Count);
            GameObject temp = listObjeto[i];
            listObjeto[i] = listObjeto[randIndex];
            listObjeto[randIndex] = temp;
        }

        for (int i = 0; i < listObjeto.Count; i++)
        {
            Objeto controller = listObjeto[i].GetComponent<Objeto>();
            controller.objectId = i; 
        }

    }

    public void ResetPuzzle()
    {
        currentObjectIndex = 0;
        for (int i = 0; i < listObjeto.Count; i++)
        {
            Objeto controller = listObjeto[i].GetComponent<Objeto>();
            controller.SpriteDefault();
        }

    }
}
