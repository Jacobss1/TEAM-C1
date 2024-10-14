using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance; // Singleton instance
    //COMPONENTS
    public BoxCollider2D boxCollider2D;
    public Rigidbody2D rb2d;
    public float PlayerWalkSpeed;
    public float playerMovement;

    //PLAYERMOVE
    public const string RIGHT = "right";
    public const string LEFT = "left";
    public float PlayerFlySpeed;
    string buttonPressed;
    public Animator animator;
    public bool isGrounded;

    //JUMP
    public float playerJumpSpeed;
    private bool isJumping;
    public float jumpCounter;
    public float jumpPower = 2f;

    //SUPERJUMP
    public float superJumpPower = 15f;
    public float launchTime = 1f;
    private bool isLaunching = false;
    private float chargeTime = 0.2f;


    //SLIDE
    [SerializeField] private TrailRenderer trailRenderer;
    private bool slide = true;
    public bool isSliding;
    public float slideSpeed = 1f;
    private float slideRange = 5f;
    private float slideTime = 1f;
    private float slideCooldown = 1f;

    //SPRINT
    public float sprintSpeed;
    public bool isSprinting;
    [SerializeField] Transform CheckGround;


    //BOX COLLIDERS

    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffSet;
    public Vector2 slideColliderSize;
    public Vector2 slideColliderOffSet;    


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        originalColliderSize = boxCollider2D.size;
        originalColliderOffSet = boxCollider2D.offset;
    }

    void Update()
    {
        if (isSliding)
        {
            return;

        }

        HandleInput();
        Jump();
        SuperJump();
        attack();

        if (Input.GetKeyDown(KeyCode.C) && isGrounded) {

            StartCoroutine(Slide());
        
        }

     

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

        if (isSliding)
        {
            animator.Play("slide");

            return;
        }
        PlayerCheckIfIsGrounded();
        PlayerMove();
       
        flyUp();
        Salute();
        // CheckLanding();


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
            }
            else

            if (isGrounded)
            {
                animator.Play("hero_idle");
                rb2d.velocity = Vector2.zero;
            }
           //animator.SetBool("isLaunching", false);
        }

        animator.SetBool("isSprinting", isSprinting);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, playerJumpSpeed);
            // animator.SetBool("isJumping", true);
            animator.Play("superJump");
            isJumping = true;
            isGrounded = false; // The player is no longer grounded after jumping
        }
        if (isGrounded && isJumping)
        {
            isJumping = false;
            //animator.SetBool("isJumping", false);
        }

      
    }



    private void SuperJump()
    {
        if (Input.GetKeyDown(KeyCode.B) && isGrounded)
        {
            isLaunching = true;
            chargeTime = Time.time; // Start charging time
           
        }

        if (Input.GetKeyUp(KeyCode.B) && isLaunching)
        {
            DoJump();
            isLaunching = false;
            animator.Play("superJump");
            //animator.SetBool("isLaunching", false);

        }
    }



    private void DoJump()
    {
        float chargeDuration = Time.time - chargeTime;
        float effectiveJumpPower = Mathf.Lerp(playerJumpSpeed, superJumpPower, Mathf.Clamp01(chargeDuration / chargeTime));
        rb2d.velocity = new Vector2(rb2d.velocity.x, effectiveJumpPower);
      
       
        isJumping = true;
        isGrounded = true;


    }
    private void flyUp()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, PlayerFlySpeed);
            
            animator.Play("super_jump");
        }
    }

    private IEnumerator Slide()
    {
        if (!isGrounded || isSliding) yield break; // Ensure sliding only happens when grounded and not already sliding

        isSliding = true;
        float originalGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0f;

        // dis change the size of the boxcollider sze & gravity;
        boxCollider2D.size = slideColliderSize;
        boxCollider2D.offset = slideColliderOffSet;


        // Determine the slide direction
        float slideDirection = buttonPressed == RIGHT ? 1f : -1f;
        rb2d.velocity = new Vector2(slideDirection * slideSpeed, 0f);

        trailRenderer.emitting = true;
        yield return new WaitForSeconds(slideTime);
        trailRenderer.emitting = false;
        boxCollider2D.size = originalColliderSize;
        boxCollider2D.offset = originalColliderOffSet;
        rb2d.gravityScale = originalGravity;
        isSliding = false;

        yield return new WaitForSeconds(slideCooldown);
    }


    private void attack()
    {
        if (Input.GetKey(KeyCode.Mouse0) && isGrounded)
        {
            animator.Play("attack");
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the object you want to respond to
        if (collision.gameObject.CompareTag("MovingObject"))
        {
            // Trigger the "hurt" animation
            animator.Play("hero_hurt");
        }
    }

  /*  private void CheckLanding()
    {
        if (isGrounded && (isJumping || isLaunching))
        {
            isJumping = false;
            isLaunching = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("islaunching", false);
        }
    }
  */
}
