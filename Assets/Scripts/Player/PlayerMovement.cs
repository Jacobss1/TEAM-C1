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
    public float playerJumpSpeed;
    public const string RIGHT = "right";
    public const string LEFT = "left";
    public float PlayerFlySpeed;
    string buttonPressed;
    public Animator animator;
    public bool isGrounded;
    [SerializeField] Transform CheckGround;

    private void Awake()
    {

    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
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
    }

    private void FixedUpdate()
    {
        PlayerCheckIfIsGrounded();
        PlayerMove();
        PlayerJump();
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
        if (buttonPressed == RIGHT)
        {
            rb2d.velocity = new Vector2(PlayerWalkSpeed, rb2d.velocity.y);
            if (isGrounded)
            {
                animator.Play("monkey_walk");
            }
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (buttonPressed == LEFT)
        {
            rb2d.velocity = new Vector2(-PlayerWalkSpeed, rb2d.velocity.y);
            if (isGrounded)
            {
                animator.Play("monkey_walk");
            }
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            if (isGrounded)
            {
                animator.Play("monkey_idle");
                rb2d.velocity = Vector2.zero;
            }
        }
    }

    private void PlayerJump()
    {
        if (Input.GetKey("space") && isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, playerJumpSpeed);
            //animator.Play("player_jump");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump_up")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump_down")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("player_double_jump_up"))
        {
            if (Input.GetKey(KeyCode.J) && !isGrounded)
            {
                animator.Play("player_jump_attack");
            }
        }
    }

    private void flyUp()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, PlayerFlySpeed);
            animator.Play("PlayerFly");
        }
    }

    private void attack()
    {
        if (Input.GetKey(KeyCode.J) && isGrounded)
        {
            animator.Play("player_attack");
            rb2d.velocity = Vector2.zero;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            this.GetComponent<PlayerMovement>().enabled = false;
        }
    }
    private void Salute()
    {
        if (Input.GetKey(KeyCode.V) && isGrounded)
        {
            animator.Play("hero_salute");
            rb2d.velocity = Vector2.zero;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            this.GetComponent<PlayerMovement>().enabled = false;

        }
    }
}

