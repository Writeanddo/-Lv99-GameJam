using System;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputReference))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Status")]
    [SerializeField] private Animator player;

    private SpriteRenderer _playerSprite;
    private Collider2D _playerCollider;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 124f;
    public bool isInteraction = false;
    
    public bool isPressedPuzzle;
    public bool isPuzzleStart;
    
    public GameObject puzzleCurrent;
    [SerializeField] private bool isLookingLeft;

    [Header("Ground")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool isGrounded;
    private bool isJumping;
    [SerializeField] private GameObject groundCheck;

    [Header("SizeGizmo")]
    [SerializeField] private float groundXSize;
    [SerializeField] private float groundYSize;

    [Header("Slopes")]
    [SerializeField] private float slopesValidadeDistance = 0.4f;
    [SerializeField] private bool isOnSlope;

    [Header("PhysicsMaterial2D")]
    [SerializeField] private PhysicsMaterial2D noFrictionMaterial;
    [SerializeField] private PhysicsMaterial2D frictionMaterial;

    [HorizontalLine(1, EColor.Green)]
    [SerializeField] private InputActionReference interactAction;

    [Header("Oxygen Player")]
    [SerializeField] private float maxPlayerOxygen = 1f;
    [SerializeField] private float currentPlayerOxygen;
    [SerializeField] private bool isLookLeft = false;

    private InputReference _inputReference;

    private Rigidbody2D _rigidbody2D;
    private IDamageable health;

    private Vector2 perpendicularSpeed;
    private float slopeAngle;


    public OxygenSystem currentCilinder;
    private bool blockPlayerInputs;

    public PlayerOxygen PlayerOxygen;

    private void Awake()
    {
        _inputReference = GetComponent<InputReference>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        player = GetComponent<Animator>();
        _playerSprite = GetComponent<SpriteRenderer>();
        _playerCollider = GetComponent<Collider2D>();
        PlayerOxygen = GetComponent<PlayerOxygen>();

        health = GetComponent<IDamageable>();
    }

    private void Start()
    {
        interactAction.action.Enable();
        interactAction.action.started += InteractStarted;
        interactAction.action.performed += InteractPerformed;
        interactAction.action.canceled += InteractCanceled;
    }

    private void Update()
    {
        player.SetFloat("speedY", _rigidbody2D.velocity.y);
         
        DetectSlopes();

        if (BlockInput())
            return;

        if (_inputReference.PauseButton.IsPressed)
        {
            Debug.Log("Pause");
            GameManager.Instance.PauseResume();
        }

        if (GameManager.Instance && GameManager.Instance.Paused)
            return;

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

        }

        if (_inputReference.interacaoButton.IsPressed && isInteraction && !isPressedPuzzle)
        {
            isPressedPuzzle = true;
            SetPuzzleStart();
        }
    }

    private bool BlockInput()
    {
        if (health.IsDie)
            return true;

        if (blockPlayerInputs)
            return true;

        return false;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(groundXSize, groundYSize), 0f, whatIsGround);

        if(BlockInput())
            return;

        OnMovimentPlayer();

        if (health.IsDie)
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

    private void InteractStarted(InputAction.CallbackContext obj)
    {
        if (BlockInput())
            return;

        if (currentCilinder == null)
            return;

        currentCilinder.StartCilinder();
    }

    private void InteractPerformed(InputAction.CallbackContext obj) 
    {
        if (BlockInput())
            return;

        if (currentCilinder == null)
            return;

        currentCilinder.HoldCilinder();
    }

    private void InteractCanceled(InputAction.CallbackContext obj) //programar o input cancel (buttonUP)
    {
        if (BlockInput())
            return;

        if (currentCilinder == null)
            return;

        currentCilinder.StopCilinder();
    }

    private void SetPuzzleStart()
    {
        isPuzzleStart = true;
        var ui = FindAnyObjectByType<UIGameplay>();
        RectTransform canvasRect = ui.canvas;

        GameObject newObject = Instantiate(puzzleCurrent);
        newObject.transform.SetParent(canvasRect, false);
        newObject.transform.localPosition = Vector3.zero;

        GameManager.Instance.TemporaryPause();
    }

    public void SetPuzzleStop()
    {
        GameManager.Instance.ResumeTemporaryPause();
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
        else
        {
            isOnSlope = false;
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

   public void OnDelayJump()
   {
        _rigidbody2D.velocity = new Vector2 (_rigidbody2D.velocity.x, 0);
   }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(groundCheck.transform.position, new Vector2(groundXSize, groundYSize));

        Gizmos.DrawRay(transform.position, Vector2.down * slopesValidadeDistance);

    }

    public void ActivePlayer()
    {
        _playerCollider.enabled = true;
        _playerSprite.enabled = true;
        blockPlayerInputs = false;
    }

    public void DesativePlayer()
    {
        _playerCollider.enabled = false;
        _playerSprite.enabled = false;
        blockPlayerInputs = true;
    }
}
