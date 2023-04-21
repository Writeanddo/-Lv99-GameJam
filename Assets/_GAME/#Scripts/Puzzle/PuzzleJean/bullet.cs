using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Transform targetObject;
    public float moveSpeed = 5f;
    public float distanceFromTarget = 1f;
    [SerializeField] private float damage = 1f;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

        Vector2 direction = (targetObject.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("puzzle"))
        {
            if (collision.gameObject.TryGetComponent(out PlayerPuzzle damageable) && collision.gameObject.CompareTag("puzzle"))
            {
                
                damageable.Stun();
                Destroy(gameObject);

            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
