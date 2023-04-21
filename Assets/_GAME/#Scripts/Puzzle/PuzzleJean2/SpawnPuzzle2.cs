using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPuzzle2 : MonoBehaviour
{

    public enum DirectionSpawn
    {
        UP, RIGHT, DOWN, LEFT
    }
    public GameObject prefabBullet;

    public DirectionSpawn direction;
    private Vector2 directionMove;
    public float moveSpeed = 5f;
    private void Awake()
    {
        ChangeDirection(direction);
    }

    private void OnEnable()
    {

        GameObject newObject = Instantiate(prefabBullet, transform.position, Quaternion.identity);
        var rb = newObject.GetComponent<Rigidbody2D>();

        rb.AddForce(directionMove * moveSpeed, ForceMode2D.Impulse);

    }

    private void ChangeDirection(DirectionSpawn direction)
    {
        switch (direction)
        {
            case DirectionSpawn.UP:
                directionMove = Vector2.up;
                break;
            case DirectionSpawn.RIGHT:
                directionMove = Vector2.right;
                break;
            case DirectionSpawn.DOWN:
                directionMove = Vector2.down;
                break;
            case DirectionSpawn.LEFT:
                directionMove = Vector2.left;
                break;


        }

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
