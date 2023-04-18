using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Transform targetObject;
    public float moveSpeed = 5f;
    public float distanceFromTarget = 1f;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D > ();
    }

    void Start()
    {
       

        Vector2 direction = (targetObject.position - transform.position).normalized;
        print(direction);


        rb.velocity = direction * moveSpeed;

    }
}
