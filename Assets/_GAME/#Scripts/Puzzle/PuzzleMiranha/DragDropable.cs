using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragDropable : MonoBehaviour
{
    public int idPuzzle;
    public Sprite icon;
    public Image slotBGPivot;
    public Image slotIcon;
    public RectTransform posInit;


    private DragAndDropManager dadManager;
    private bool isBeingUsed; // está sendo usado
    private int idTarget = -1;

    private void Start()
    {
        dadManager = DragAndDropManager.Instance;
        slotBGPivot.sprite = icon;
        slotIcon.sprite = icon;
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
        
        // VERIFICA SE ESTÁ PERTO DO SLOT ALVO
        if (shorterDistance < 50 && dadManager.targetSlots[idTemp].isOccupied == false)
        {
            if(idTarget != idTemp && idTarget != -1)
            {
                dadManager.targetSlots[idTarget].isOccupied = false;
                dadManager.RemoveListCells(idPuzzle, idTarget);
            }
            idTarget = idTemp;
            transform.position = dadManager.targetSlots[idTarget].target.transform.position;
            dadManager.targetSlots[idTarget].isOccupied = true;
            isBeingUsed = true;
            dadManager.AddIdListCells(idPuzzle,idTarget);
        }
        else // NÃO ESTÁ PERTO DO SLOT DE RESOLUÇÃO
        {
            transform.position = posInit.position;
            if (isBeingUsed)
            {
                isBeingUsed = false;
                dadManager.targetSlots[idTarget].isOccupied = false;
                dadManager.RemoveListCells(idPuzzle, idTarget);
            }
        }
    }

}
