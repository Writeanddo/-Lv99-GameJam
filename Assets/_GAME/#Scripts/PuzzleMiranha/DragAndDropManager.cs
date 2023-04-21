using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager : Singleton<DragAndDropManager>
{

    public List<InfosTargetSlot> targetSlots;
    public InputReference inputReference;
    // Start is called before the first frame update
    void Start()
    {
        inputReference = GetComponent<InputReference>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class InfosTargetSlot
{
    public GameObject target; 
    public int idCorrect; // ID CORRETO PARA ESTE PUZZLE
    public bool isOccupied; // ESTÁ OCUPADO
}
