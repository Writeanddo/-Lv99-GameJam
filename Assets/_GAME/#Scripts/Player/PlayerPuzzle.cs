using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


[RequireComponent(typeof(InputReference))]
public class PlayerPuzzle : MonoBehaviour
{

    [Header("Player Status")]
    public float moveSpeed = 5f;
    public GameObject stunPrefab;

    [HorizontalLine(1, EColor.Green)]

    [SerializeField] private bool isLookingLeft;

    private InputReference _inputReference;
    private Rigidbody2D _rigidbody2D;


    [SerializeField] private bool isStunned;
    [SerializeField] private bool isLookLeft = false;
    [SerializeField] private float stunnedTime = 1;
    [SerializeField] private float stunForce = 15;

    private void Awake()
    {
        _inputReference = GetComponent<InputReference>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }



    public void Stun()
    {
        isStunned = true;
        stunPrefab.SetActive(true);
        _rigidbody2D.velocity = Vector2.zero;
        StartCoroutine(IE_Stun());
    }

    private IEnumerator IE_Stun()
    {
        yield return new WaitForSeconds(stunnedTime);
        stunPrefab.SetActive(false);
        isStunned = false;


    }

    private void Update()
    {


      

        if (_inputReference.PauseButton.IsPressed)
        {

            Debug.Log("Pause");
            GameManager.Instance.PauseResume();
        }

        if (GameManager.Instance && GameManager.Instance.Paused)
            return;

        if (isStunned)
            return;


    }
    private void FixedUpdate()
    {



        if (isStunned )
            return;

        OnMovimentPlayer();

       

        //virar o player
        if (_inputReference.Movement.x < 0 && isLookLeft == false)
        {


            Flip();
        }
        else if (_inputReference.Movement.x > 0 && isLookLeft == true)
        {

            Flip();
        }

    }


    private void OnMovimentPlayer()
    {


        //Movimentacao Padrao
        _rigidbody2D.velocity = _inputReference.Movement * moveSpeed;


    }


    public void Flip()
    {
       
            isLookLeft = !isLookLeft;
            float x = transform.localScale.x * -1; //Inverte o sinal do scale X
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
       

    }



}
