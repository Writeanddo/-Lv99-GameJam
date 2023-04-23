using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGasWalk : MonoBehaviour
{

    public float moveSpeed = 2f;
    public float moveSpeedFollow = 2f;
    public float ocilacaoSpeed = 1f;
    public float ocilacaoDistance = 0.5f;
    public float damage;

    [SerializeField]
    private float positionY;
    private float floatOffset;

    private float timer = 0f;
    public float patrolTime = 3f;

    [SerializeField]
    private bool isLookLeft = false;

    private IAVisionCircle m_IaVisionCircle;
    public Collider2D[] hitInfo;
    public bool isFollow;
    private Vector2 initialPosition;

    //TESTE
    private Transform lastPlayerPosition;
    public bool isTouchPlayer;


    void Start()
    {
        initialPosition = transform.position;
        m_IaVisionCircle = GetComponent<IAVisionCircle>();
        positionY = transform.position.y;
        floatOffset = Random.Range(0, 2 * Mathf.PI);
    }


    private void FixedUpdate()
    {
        if (m_IaVisionCircle != null ) 
        {
            hitInfo = Physics2D.OverlapCircleAll(m_IaVisionCircle.hitBox.position, m_IaVisionCircle.visionRange, m_IaVisionCircle.hitMask);

            if (hitInfo.Length != 0 && !isTouchPlayer)
            {
                Transform curentTarget = this.transform;
                float distanceToTarget = Mathf.Infinity;
                for (int i = 0; i < hitInfo.Length; i++)
                {
                    float newDistance = (hitInfo[i].transform.position - transform.position).magnitude;
                    if (newDistance < distanceToTarget)
                    {
                        curentTarget = hitInfo[i].transform;
                        distanceToTarget = newDistance;
                    }
                }
                lastPlayerPosition = curentTarget;
                isTouchPlayer = true;

            }
            else if(hitInfo.Length == 0 && isTouchPlayer)
            {
                isTouchPlayer = false;
                lastPlayerPosition = null;
            }

        }

      

    }

    void Update()
    {



       
        //if (m_IaVisionCircle != null)
        //{

        //    hitInfo = Physics2D.OverlapCircleAll(m_IaVisionCircle.hitBox.position, m_IaVisionCircle.visionRange, m_IaVisionCircle.hitMask);
        //    foreach (var hit in hitInfo)
        //    {
        //        isFollow = Check(hit);

        //    }

        //}

        if (moveSpeed < 0 && isLookLeft == false && lastPlayerPosition == null)
        {

            Flip();
        }
        else if (moveSpeed > 0 && isLookLeft == true && lastPlayerPosition == null)
        {

            Flip();
        }



        if (lastPlayerPosition == null)
        {
            positionY = transform.position.y;
            transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0));
            float y = positionY + ocilacaoDistance * Mathf.Sin((Time.time + floatOffset) * ocilacaoSpeed);
            transform.position = new Vector2(transform.position.x, y);

            timer += Time.deltaTime;
            if (timer >= patrolTime)
            {
                moveSpeed *= -1;
                timer = 0f;
                initialPosition = transform.position;

            }
        }

        //se movimenta até o player localizado
        if (lastPlayerPosition != null )
        {

            transform.position = Vector2.MoveTowards(transform.position, lastPlayerPosition.transform.position, moveSpeedFollow * Time.deltaTime);
        }

        //if (isFollow && hitInfo.Length>0)
        //{

        //     transform.position = Vector3.MoveTowards(transform.position, hitInfo[0].transform.position, moveSpeed * Time.deltaTime);

        //}

        if (lastPlayerPosition != null)
        {
            if (isTouchPlayer)
            {
                if (lastPlayerPosition.transform.position.x < transform.position.x && isLookLeft == false)
                {
                    Flip();
                }
                else if (lastPlayerPosition.transform.position.x > transform.position.x && isLookLeft == true)
                {
                   Flip();

                }
            }
        }

        //if (hitInfo.Length > 0 && hitInfo[0].transform.position.x > transform.position.x && isLookLeft)
        //{
        //    Flip();
        //}
        //else if (hitInfo.Length > 0 &&  hitInfo[0].transform.position.x < transform.position.x && !isLookLeft)
        //{
        //    Flip();
        //}
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
        initialPosition = transform.position;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageable) && collision.gameObject.layer != LayerMask.NameToLayer("invencivelPlayer"))
            {
                
                damageable.TakeDamage(transform.position, damage);
            

            }
        }
    }

}
