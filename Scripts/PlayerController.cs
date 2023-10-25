using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float RunSpeed = 8f;

    Vector2 moveInput;

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving { 
        get
        {
            return _isMoving;
        } 
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        } 
    }

    [SerializeField]
    private bool _isJump = false;

    public bool IsJump
    {
        get
        {
            return _isJump;
        }
        private set
        {
            _isJump = value;
            animator.SetBool("isJump", value);
        }
    }

    public bool _isFacingRight = true;


    public bool IsFacingRight 
    { get 
        {
            return _isFacingRight;
        }
    private set 
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } }

    private bool jumping;

    private float jumptime;

    private float maxJumpTime = 1f;

    public float gravityScale = 1f;

    public float fallingGravityScale = 4.5f;

    public float jumpHeight = 2f    ;

    [SerializeField]
    private bool isGrounded = true;
    public Transform feetPos;
    public float checkRadius;

    [SerializeField]
    public LayerMask _Ground;

    Rigidbody2D rb;

    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, _Ground);

        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravityScale;
        }
        rb.velocity = new Vector2(moveInput.x * RunSpeed, rb.velocity.y);


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumping = true;
            float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        if (jumping)
        {
            jumptime += Time.deltaTime;   
        }

        if (Input.GetKeyUp(KeyCode.Space) | jumptime > maxJumpTime)
        {
            jumping = false;
        }
    }

    private void FixedUpdate()
    { 
    }
      
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFaceDirection(moveInput);
    }

    private void SetFaceDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;

        } else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsJump = true;
        }
        else if (context.canceled)
        {
            IsJump = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
    }
} 
