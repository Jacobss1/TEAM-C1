using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance; // Singleton instance

    public BoxCollider2D boxCollider2D;
    public Rigidbody2D rb2d;
    public float PlayerWalkSpeed;
    public float playerMovement;

    public const string RIGHT = "right";
    public const string LEFT = "left";
    public float PlayerFlySpeed;
    string buttonPressed;
    public Animator animator;
    public bool isGrounded;
    public float playerJumpSpeed;
    private bool isJumping;
    public float jumpCounter;
    public float jumpPower = 2f;

    public float sprintSpeed;
    public bool isSprinting;
    [SerializeField] Transform CheckGround;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
      

        HandleInput();
        Jump();

    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            buttonPressed = RIGHT;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            buttonPressed = LEFT;
        }
        else
        {
            buttonPressed = null;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    private void FixedUpdate()
    {
        PlayerCheckIfIsGrounded();
        PlayerMove();
        attack();
        flyUp();
        Salute();
       
    }

    private void PlayerCheckIfIsGrounded()
    {
        float groundCheckRadius = 0.2f;
        LayerMask groundLayer = LayerMask.GetMask("ground");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(CheckGround.position, groundCheckRadius, groundLayer);
        isGrounded = colliders.Length > 0;

        Debug.DrawRay(CheckGround.position, Vector2.down * groundCheckRadius, Color.red);
        Debug.Log("Is Grounded: " + isGrounded);
    }

    private void PlayerMove()
    {
        float moveSpeed = isSprinting ? sprintSpeed : PlayerWalkSpeed;

        if (buttonPressed == RIGHT)
        {
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (buttonPressed == LEFT)
        {
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (isGrounded)
        {
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        }

        // Handle animations
        if (isGrounded && !isJumping) // Only play walk/idle animations if not jumping
        {
            if (isSprinting)
            {
                animator.Play("hero_run");
            }
            else if (buttonPressed == LEFT || buttonPressed == RIGHT)
            
            {
                animator.Play("player_walk");
            }else
             
            if(isGrounded)
            {
                animator.Play("hero_idle");
                rb2d.velocity = Vector2.zero;
            }
        }

        animator.SetBool("isSprinting", isSprinting);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.B) && isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, playerJumpSpeed);
            animator.Play("player_jump");
            isJumping = true;
            isGrounded = false; // The player is no longer grounded after jumping
        }
        if (isGrounded && isJumping)
        {
            isJumping = false;
        }

        animator.SetBool("isJumping", isJumping);
    }   
        private void flyUp()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, PlayerFlySpeed);
            //animator.Play("PlayerFly");
        }
    }

    private void attack()
    {
        if (Input.GetKey(KeyCode.J) && isGrounded)
        {
            animator.Play("player_attack");
            rb2d.velocity = Vector2.zero;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            this.enabled = false;
        }
    }

    private void Salute()
    {
        if (Input.GetKey(KeyCode.V) && isGrounded)
        {
            animator.Play("hero_salute");
            rb2d.velocity = Vector2.zero;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            this.enabled = false;
        }
    }
}
