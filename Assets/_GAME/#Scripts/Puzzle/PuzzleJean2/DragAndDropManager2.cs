using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager2 : MonoBehaviour
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
