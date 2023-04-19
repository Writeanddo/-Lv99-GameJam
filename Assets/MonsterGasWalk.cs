using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGasWalk : MonoBehaviour
{

    public float moveSpeed = 2f;
    public float ocilacaoSpeed = 1f;
    public float ocilacaoDistance = 0.5f;

    private float positionX;
    private float positionY;
    private float floatOffset;

    private float timer = 0f;
    public float patrolTime = 3f;
    private bool isLookLeft = false;

    void Start()
    {
        positionX = transform.position.x;
        positionY = transform.position.y;
        floatOffset = Random.Range(0, 2 * Mathf.PI);
    }


    void Update()
    {

        if (moveSpeed < 0 && isLookLeft == false)
        {


            Flip();
        }
        else if (moveSpeed > 0 && isLookLeft == true)
        {

            Flip();
        }

        transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0));

        float y = positionY + ocilacaoDistance * Mathf.Sin((Time.time + floatOffset) * ocilacaoSpeed);
        transform.position = new Vector2(transform.position.x, y);

        // Contagem do tempo de patrulha
        timer += Time.deltaTime;
        if (timer >= patrolTime)
        {
            // Mudança de direção
            moveSpeed *= -1;
            timer = 0f;
        }
    }

    public void Flip()
    {

        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1; //Inverte o sinal do scale X
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);


    }
}
