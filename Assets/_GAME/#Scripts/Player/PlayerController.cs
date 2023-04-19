using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


[RequireComponent(typeof(InputReference))]
public class PlayerController : MonoBehaviour
{

    [Header("Player Status")]
    public float moveSpeed = 5f;
    public float jumpForce = 124f;

    [Header("Ground")]
    public LayerMask whatIsGround;
    public bool isGrounded;
    private bool isJumping;
    public GameObject groundCheck;

    [Header("SizeGizmo")]
    public float groundXSize;
    public float groundYSize;

    [Header("Slopes")]
    [SerializeField] private float slopesCheckDistance;
    private Vector2 position;
    private Vector2 colliderSize;
    private Vector2 perpendicularSpeed;
    private CapsuleCollider2D m_collider;
    private float slopeAngle;
    [SerializeField] private bool isOnSlope;


    [Header("PhysicsMaterial2D")]
    [SerializeField] private PhysicsMaterial2D noFrictionMaterial;
    [SerializeField] private PhysicsMaterial2D frictionMaterial;

    [HorizontalLine(1, EColor.Green)]

    private Camera main;

    private Vector3 mouseWorldPosition;
    [SerializeField] private bool isLookingLeft;

    private InputReference _inputReference;
    private Rigidbody2D _rigidbody2D;

    private IDamageable health;

    public Animator player;

    [SerializeField] private bool isStunned;
    [SerializeField] private bool isLookLeft = false;
    [SerializeField] private float stunnedTime = 1;
    [SerializeField] private float stunForce = 15;

    private void Awake()
    {
        _inputReference = GetComponent<InputReference>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        player = GetComponent<Animator>();
        health = GetComponent<IDamageable>();
        m_collider = GetComponent<CapsuleCollider2D>();
        colliderSize = m_collider.size;
    }

    private void Start()
    {
        main = Camera.main;

        health.OnTakeDamage += Stun;
    }

    private void OnDestroy()
    {
        health.OnTakeDamage -= Stun;
    }

    private void Stun(Vector3 direction)
    {
        isStunned = true;

        var dir = transform.position - direction;

        _rigidbody2D.AddForce(dir.normalized * stunForce);

        StartCoroutine(IE_Stun());
    }

    private IEnumerator IE_Stun()
    {
        yield return new WaitForSeconds(stunnedTime);

    }

    private void Update()
    {
        DetectSlopes();

        position = transform.position - new Vector3(0f, colliderSize.y / 2, 0f);

        if (health.IsDie)
            return;

        if (_inputReference.PauseButton.IsPressed)
        {

            Debug.Log("Pause");
            GameManager.Instance.PauseResume();
        }

        if (GameManager.Instance && GameManager.Instance.Paused)
            return;

        if (isStunned)
            return;

        mouseWorldPosition = main.ScreenToWorldPoint(_inputReference.MousePosition);

        //tá estranho
        if (_inputReference.JumpButton.IsPressed && isGrounded && !isJumping)
        {

            JUMP();
        }
        else if(!isGrounded && isJumping)
        {
            CANCELJUMP();
        }
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(groundXSize, groundYSize), 0f, whatIsGround);

        OnMovimentPlayer();

        if (isStunned || health.IsDie)
            return;


        if (_rigidbody2D.velocity != new Vector2(0, 0))
        {
            player.SetBool("isWalk", true);
        }
        else
        {
            player.SetBool("isWalk", false);
        }

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

        if (isOnSlope && !isJumping)
        {
            //Movimentacao SLOP
            _rigidbody2D.velocity = new Vector2(-_inputReference.Movement.x * moveSpeed * perpendicularSpeed.x,
                -_inputReference.Movement.x * moveSpeed * perpendicularSpeed.y);
        }
        else
        {
            //Movimentacao Padrao
            _rigidbody2D.velocity = new Vector2(_inputReference.Movement.x * moveSpeed, _rigidbody2D.velocity.y);

        }
    }

    public void DetectSlopes()
    {
        RaycastHit2D hitSlope = Physics2D.Raycast(position, Vector2.down, slopesCheckDistance, whatIsGround);

        if (hitSlope)
        {
            perpendicularSpeed = Vector2.Perpendicular(hitSlope.normal).normalized;
            slopeAngle = Vector2.Angle(hitSlope.normal, Vector2.up);
            isOnSlope = slopeAngle != 0;
        }


        if (isOnSlope && _inputReference.Movement.x == 0)
        {
            _rigidbody2D.sharedMaterial = frictionMaterial;
        }
        else
        {
            _rigidbody2D.sharedMaterial = noFrictionMaterial;
        }
    }
    public void Flip()
    {
        if (health.IsDie == false)
        {
            isLookLeft = !isLookLeft;
            float x = transform.localScale.x * -1; //Inverte o sinal do scale X
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }

    }
    public void JUMP()
    {

        if (isGrounded == true)
        {
            isJumping = true;
            isGrounded = false;
            _rigidbody2D.AddForce(new Vector2(0, jumpForce));
        }
    }


    public void CANCELJUMP()
    {
        isJumping = false;
        //if (_rigidbody2D.velocity.y > 0)
        //{
        //    StartCoroutine("delayJump");

        //}
    }

    IEnumerator delayJump()
    {
        yield return new WaitForSeconds(0.05f);

        _rigidbody2D.velocity = Vector2.up * 0.3f;

        StopCoroutine("delayJump");
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(groundCheck.transform.position, new Vector2(groundXSize, groundYSize));

    }

}
