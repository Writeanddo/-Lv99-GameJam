using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragDropable : MonoBehaviour
{
    public RectTransform posInit;
    private DragAndDropManager dadManager;
    private bool isBeingUsed; // está sendo usado
    private int idTarget = -1;
    private void Start()
    {
        dadManager = DragAndDropManager.Instance;
    }

    public void DragItem()
    {
        transform.position = DragAndDropManager.Instance.inputReference.MousePosition;
    }

    public void EndDragItem()
    {
        float shorterDistance = Mathf.Infinity;
        int idTemp = -1;
        for (int i = 0; i < DragAndDropManager.Instance.targetSlots.Count; i++)
        {
            float dist = Vector3.Distance(DragAndDropManager.Instance.targetSlots[i].target.transform.position, transform.position);
            if (dist < shorterDistance)
            {
                shorterDistance = dist;
                idTemp = i;
            }
        }
        
        if (shorterDistance < 50 && dadManager.targetSlots[idTemp].isOccupied == false)
        {
            if(idTarget != idTemp && idTarget != -1)
            {
                dadManager.targetSlots[idTarget].isOccupied = false;
            }
            idTarget = idTemp;
            transform.position = dadManager.targetSlots[idTarget].target.transform.position;
            dadManager.targetSlots[idTarget].isOccupied = true;
            isBeingUsed = true;
        }
        else
        {
            transform.position = posInit.position;
            if (isBeingUsed)
            {
                isBeingUsed = false;
                dadManager.targetSlots[idTarget].isOccupied = false;
            }
        }
    }

}
