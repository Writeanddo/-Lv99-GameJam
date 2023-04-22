using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputReference))]
public class PlayerController : MonoBehaviour
{

    [Header("Player Status")]
    public float moveSpeed = 5f;
    public float jumpForce = 124f;
    public bool isInteraction = false;
    public bool isPressedPuzzle;
    public bool isPuzzleStart;
    public GameObject puzzleCurrent;


    [Header("Ground")]
    public LayerMask whatIsGround;
    public bool isGrounded;
    private bool isJumping;
    public GameObject groundCheck;

    [Header("SizeGizmo")]
    public float groundXSize;
    public float groundYSize;

    [Header("Slopes")]
    [SerializeField] private float slopesValidadeDistance = 0.4f;
    private Vector2 perpendicularSpeed;
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
    public float speedY;


    public InputActionReference interactAction;

    [Header("OxigenAction")]
    public bool isOxygenStart;
    public float oxigenCilinderValue;
    public SpriteRenderer barOxigenSr;

    [Header("OxigenHUD")]


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
    }

    private void Start()
    {
        main = Camera.main;

        health.OnTakeDamage += Stun;
        interactAction.action.Enable();
        interactAction.action.started += interactStarted;
        interactAction.action.performed += interactPerformed;
        interactAction.action.canceled += InteractCanceled;
    }

    private void interactPerformed(InputAction.CallbackContext obj) //start
    {
        //throw new NotImplementedException();
        print("Apertou botao");
        if (isOxygenStart == false)
        {
            isOxygenStart = true;
            StartCoroutine("OxygenBar");
        }

    }

    private void interactStarted(InputAction.CallbackContext obj) // durante
    {
        //throw new NotImplementedException();
        print("botao segurado");
  

    }

    private void InteractCanceled(InputAction.CallbackContext obj) //programar o input cancel (buttonUP)
    {
     
        oxigenCilinderValue = 0;
        StopCoroutine("OxigenBar");
        isOxygenStart = false;
         oxigenCilinderValue = 0;
        barOxigenSr.size = new Vector2(0.56f, oxigenCilinderValue);
        print("oxigenio zerado");
        print("soltou Botao");
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
         player.SetFloat("speedY", _rigidbody2D.velocity.y);
         
        DetectSlopes();

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

        //t� estranho

        if (isGrounded && isJumping)
        {
            
            CANCELJUMP();
        }

        if (_inputReference.JumpButton.IsPressed && isGrounded && !isJumping)
        {
            JUMP();
        }

        if(_inputReference.JumpButton.IsPressed == false && isGrounded == false )
        {
            //print("soltando jump");
            //OnDelayJump();
        }

        if (_inputReference.interacaoButton.IsPressed && isInteraction && !isPressedPuzzle)
        {
            isPressedPuzzle = true;
            SetPuzzleStart();
        }

    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(groundXSize, groundYSize), 0f, whatIsGround);

        if (isPuzzleStart)
        {
            return;
        }

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

        player.SetBool("isGrounded", isGrounded);
    }


    private void SetPuzzleStart()
    {
        isPuzzleStart = true;
        var ui = FindAnyObjectByType<UIGameplay>();
        RectTransform canvasRect = ui.canvas;
        GameObject newObject = Instantiate(puzzleCurrent);
        newObject.transform.SetParent(canvasRect, false);
        newObject.transform.localPosition = Vector3.zero;
        print("Pausou");
    }

    private void OnMovimentPlayer()
    {

      if (isOnSlope)
        {
            if(isJumping)
            {
                isOnSlope = false;
            }
            else
            {
                //Movimentacao SLOP
                _rigidbody2D.velocity = new Vector2(-_inputReference.Movement.x * moveSpeed * perpendicularSpeed.x,
                    -_inputReference.Movement.x * moveSpeed * perpendicularSpeed.y);

            }
        }
        else
        {
            //Movimentacao Padrao
            _rigidbody2D.velocity = new Vector2(_inputReference.Movement.x * moveSpeed, _rigidbody2D.velocity.y);

        }
    }

    public void DetectSlopes()
    {
        RaycastHit2D hitSlope = Physics2D.Raycast(groundCheck.transform.position, Vector2.down, slopesValidadeDistance, whatIsGround);

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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(groundCheck.transform.position, new Vector2(groundXSize, groundYSize));

        Gizmos.DrawRay(transform.position, Vector2.down * slopesValidadeDistance);

    }

   public void OnDelayJump()
   {
        _rigidbody2D.velocity = new Vector2 (_rigidbody2D.velocity.x, 0);
   }


    IEnumerator OxygenBar()
    {
        
       
        for (int i = 0; i < 100; i++)
        {
            if (isOxygenStart == true)
            {
     
                oxigenCilinderValue += 0.01f;
                print(oxigenCilinderValue);
                barOxigenSr.size = new Vector2(0.56f, oxigenCilinderValue);
                yield return new WaitForSeconds(0.01f);
            }
        }



        for (int i = 0; i < 100; i++)
        {
            if (isOxygenStart == true)
            {
         
                oxigenCilinderValue -= 0.01f;
                print(oxigenCilinderValue);
                barOxigenSr.size = new Vector2(0.56f, oxigenCilinderValue);
                yield return new WaitForSeconds(0.01f);
            }

        }

        if (isOxygenStart == true)
        {
            StartCoroutine("OxygenBar");
        }

        else
        {
            print("end Cilinder");
            isOxygenStart = false;
        }



    }
}
