using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField] private float sprintSpeed = 10f;  // Sprint speed
    [SerializeField] private float normalSpeed = 5f;   // Normal walking speed
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    Container con;
    private Rigidbody2D rb;
    public Animator anim;
    public BoxCollider2D boxCollider;

    private float wallJumpCooldown;
    public float horizontalInput;

    // Crouch variables
    public KeyCode crouchKey = KeyCode.C;
    public float crouchSpeed = 2f;
    public bool isCrouching = false;

    public Vector2 originalColliderSize;
    public Vector2 originalColliderOffset;
    public Vector2 slideColliderSize;
    public Vector2 slideColliderOffset;

    public bool isJumping = false; // Flag to track if the player is jumping
    public float groundedBufferTime = 0.1f;
    public float groundedBufferCounter = 0f;
    public bool isGroundCheckBuffered = false;

    // KnockBack
    private KnockBack knockBack;
    private void Start()
    {

        rb = Container.Instance.rb;
    }
    private void Awake()
    {
      //  con.rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        knockBack = GetComponent<KnockBack>();

        // Store original collider size and offset
        originalColliderSize = boxCollider.size;
        originalColliderOffset = boxCollider.offset;
    }

    private void Update()
    {
        if (!knockBack.isBeingKnockBack)
        {
            // Get horizontal movement input
            horizontalInput = Input.GetAxis("Horizontal");

            // Handle sprint
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                moveSpeed = sprintSpeed;  // Sprint speed when shift is held
                anim.SetBool("isRunning",  true);  // Set running animation
              //  anim.SetBool("isWalking", true);
           
            }
            else
            {
               moveSpeed = normalSpeed;  // Normal walking speed when shift is not held
                anim.SetBool("isRunning", false);
                
            }

            // Movement logic
            if (isGrounded() || !onWall())
            {
                anim.SetBool("isWallClimbing", false);
            }

            // Flip player sprite based on movement direction
            if (horizontalInput > 0.01f)
                transform.localScale = Vector3.one;
            else if (horizontalInput < -0.01f)
                transform.localScale = new Vector3(-1, 1, 1);

            // Set animator parameters
            anim.SetBool("isWalking", horizontalInput != 0 && !isCrouching && !isJumping);
            anim.SetBool("isGrounded", isGrounded());

            // Wall jump cooldown logic
            if (wallJumpCooldown > 0.2f)
            {
                rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

                if (onWall() && !isGrounded())
                {
                    rb.gravityScale = 1.5f;
                    rb.velocity = Vector2.zero;
                }
                else
                {
                    rb.gravityScale = 2;
                }

                if (Input.GetKey(KeyCode.Space))
                    Jump();
            }
            else
            {
                wallJumpCooldown += Time.deltaTime;
            }

            // Crouch logic
            if (Input.GetKey(crouchKey) && isGroundBuffered())
            {
                Crouch();
            }
            else if (Input.GetKeyUp(crouchKey))
            {
                StopCrouching();
            }
        }
    }

    // Jump logic
    public void Jump()
    {
        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            anim.SetBool("isWallClimbing", false);
            isJumping = true;
            anim.SetTrigger("Jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 1, 6);
            }

            wallJumpCooldown = 1f;
            anim.SetBool("isWallClimbing", true);
        }
    }

    // Crouch logic
    private void Crouch()
    {
        if (!isCrouching)
        {
            isCrouching = true;
            groundedBufferCounter = groundedBufferTime;
            isGroundCheckBuffered = true;
            boxCollider.size = slideColliderSize;
            boxCollider.offset = slideColliderOffset;
            anim.SetBool("isCrouching", true);
        }
        rb.velocity = new Vector2(horizontalInput * crouchSpeed, rb.velocity.y);
        anim.SetBool("isCrouchWalking", horizontalInput != 0);
    }

    private void StopCrouching()
    {
        if (isCrouching)
        {
            isCrouching = false;
            boxCollider.size = originalColliderSize;
            boxCollider.offset = originalColliderOffset;
            anim.SetBool("isCrouching", false);
            anim.SetBool("isCrouchWalking", false);
        }
    }

    public bool isGroundBuffered()
    {
        if (groundedBufferCounter > 0)
        {
            groundedBufferCounter -= Time.deltaTime;
            return true;
        }
        return isGrounded();
    }

    public bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        isJumping = !raycastHit.collider;
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
