using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGasWalk : MonoBehaviour
{

    public float moveSpeed = 2f;
    public float ocilacaoSpeed = 1f;
    public float ocilacaoDistance = 0.5f;
    public float damage;

    [SerializeField]
    private float positionY;
    private float floatOffset;

    private float timer = 0f;
    public float patrolTime = 3f;
    private bool isLookLeft = false;

    private IAVisionCircle m_IaVisionCircle;
    public Collider2D[] hitInfo;
    public bool isFollow;
    public bool Patrulha;
    private Vector2 initialPosition;


    [SerializeField] private float followDelayTime = 2f; // tempo de atraso em segundos
    [SerializeField] private float followDelayTimer = 0f;

    void Start()
    {
        initialPosition = transform.position;
        m_IaVisionCircle = GetComponent<IAVisionCircle>();
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
                if (followDelayTimer >= followDelayTime)
                {
                    isFollow = Check(hit);
                    followDelayTimer = 0f;
                    break;

                }
            }

        }

        if (moveSpeed < 0 && isLookLeft == false && !isFollow)
        {
            print("Patrulha ESQUERDA");
            Flip();
        }
        else if (moveSpeed > 0 && isLookLeft == true && !isFollow)
        {

            print("Patrulha DIREITA");
            Flip();
        }



        if (!isFollow)
        {
            Patrulha = true;
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
        else if (isFollow && hitInfo.Length>0)
        {
            Patrulha = false;
            if (hitInfo[0].transform.position.x > transform.position.x && isLookLeft)
            {
                Debug.Log("<color=red> ANTES DIREITA: " + moveSpeed + "</color>");
                if (moveSpeed <= 0)
                    moveSpeed *= -1;

                Flip();
                Debug.Log("<color=green> DEPOIS DIREITA: " + moveSpeed + "</color>");
            }
            else if (hitInfo[0].transform.position.x < transform.position.x && !isLookLeft)
            {
                Debug.Log("<color=blue> ANTES EQUERDA: " + moveSpeed + "</color>");
                //if (moveSpeed >= 0)
                //    moveSpeed *= -1;
                Flip();
                Debug.Log("<color=magenta> DEPOIS EQUERDA: " + moveSpeed + "</color>");
            }
            transform.position = Vector3.MoveTowards(transform.position, hitInfo[0].transform.position, moveSpeed * Time.deltaTime);

            // verificar se o jogador está fora do alcance de visão do inimigo
            if (!m_IaVisionCircle.IsVisible(hitInfo[0]))
            {
                // jogador está fora do alcance, iniciar o temporizador de atraso
                isFollow = false;
                followDelayTimer = 0f;
            }
        }

        // atualizar o temporizador de atraso
        if (followDelayTimer < followDelayTime)
        {
            followDelayTimer += Time.deltaTime;
            if (followDelayTimer >= followDelayTime)
            {
                // tempo de atraso atingido, redefinir isFollow para false
                isFollow = false;
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
