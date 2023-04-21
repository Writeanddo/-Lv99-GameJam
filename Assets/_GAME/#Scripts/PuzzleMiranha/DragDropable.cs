using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragDropable : MonoBehaviour
{
    public RectTransform posInit;

    private void Start()
    {
        
    }

    public void DragItem()
    {
        transform.position = DragAndDropManager.Instance.inputReference.MousePosition;
    }

    public void EndDragItem()
    {
        float shorterDistance = Mathf.Infinity;
        int idTarget = -1;

        for (int i = 0; i < DragAndDropManager.Instance.targetSlots.Count; i++)
        {
            float dist = Vector3.Distance(DragAndDropManager.Instance.targetSlots[i].transform.position, transform.position);
            if (dist < shorterDistance)
            {
                shorterDistance = dist;
                idTarget = i;
            }
        }

        if (shorterDistance < 50)
        {
            transform.position = DragAndDropManager.Instance.targetSlots[idTarget].transform.position;
        }
        else
        {
            transform.position = posInit.position;
        }
    }

}
