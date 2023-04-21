using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GismoColor : MonoBehaviour
{
    [Header("SizeGizmo")]
    public float groundXSize;
    public float groundYSize;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        groundXSize = boxCollider2D.size.x;
        groundYSize = boxCollider2D.size.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector2(groundXSize, groundYSize));

    }
   
}
