using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objeto : MonoBehaviour
{

    public int objectId;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite[] spriteChange;

    private PointsOrderPuzzle m_PointsOrderPuzzle;
    private bool isCorret;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        m_PointsOrderPuzzle = GetComponentInParent<PointsOrderPuzzle>();
    }

    public void SpriteDefault()
    {
        spriteRenderer.sprite = spriteChange[0];
        isCorret = false;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCorret)
        {

            if (m_PointsOrderPuzzle.currentObjectIndex == objectId)
            {
                isCorret = true;
                spriteRenderer.sprite = spriteChange[1];
                m_PointsOrderPuzzle.currentObjectIndex++;
            }
            else
            {
                m_PointsOrderPuzzle.ResetPuzzle();
            }

        }
    }


}

