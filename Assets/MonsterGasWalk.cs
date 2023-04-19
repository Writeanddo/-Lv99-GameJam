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

    private IAVisionCircle m_IaVisionCircle;
    public Collider2D[] hitInfo;
    public bool isFollow;

    void Start()
    {
        m_IaVisionCircle = GetComponent<IAVisionCircle>();
        positionX = transform.position.x;
        positionY = transform.position.y;
        floatOffset = Random.Range(0, 2 * Mathf.PI);
    }


    void Update()
    {
        if (m_IaVisionCircle != null)
        {

            hitInfo = Physics2D.OverlapCircleAll(m_IaVisionCircle.hitBox.position, m_IaVisionCircle.visionRange, m_IaVisionCircle.hitMask);
            foreach (var hit in hitInfo)
            {
                isFollow = Check(hit);

            }

        }

        if (moveSpeed < 0 && isLookLeft == false && !isFollow)
        {

            Flip();
        }
        else if (moveSpeed > 0 && isLookLeft == true && !isFollow)
        {

            Flip();
        }

        if (isFollow && hitInfo !=null)
        {
            transform.position = Vector3.MoveTowards(transform.position, hitInfo[0].transform.position, moveSpeed * Time.deltaTime);
            if (hitInfo[0].transform.position.x > transform.position.x && isLookLeft)
            {
                Flip();
            }
            else if (hitInfo[0].transform.position.x < transform.position.x && !isLookLeft)
            {
                Flip();
            }
        }

        if (!isFollow)
        {
            transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0));
            float y = positionY + ocilacaoDistance * Mathf.Sin((Time.time + floatOffset) * ocilacaoSpeed);
            transform.position = new Vector2(transform.position.x, y);

            timer += Time.deltaTime;
            if (timer >= patrolTime)
            {
                moveSpeed *= -1;
                timer = 0f;
            }
        }

    }

    private bool Check(Collider2D colPlayer)
    {

        return m_IaVisionCircle.IsVisible(colPlayer);

    }

    public void Flip()
    {

        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1; //Inverte o sinal do scale X
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);


    }
}
