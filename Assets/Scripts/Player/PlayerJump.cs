using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    Animator animator;
    public Rigidbody2D rb2d;
    public float playerJumpSpeed;
    private bool isJumping;
    public float jumpCounter;
    public float jumpPower = 2f;
    public static PlayerMovement instance;


    void Start()
    {
        rb2d.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Jump();


    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.B) && PlayerMovement.instance.isGrounded)
        {
            isJumping = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, playerJumpSpeed);
            animator.Play("player_jump");
        }
        if (Input.GetKey(KeyCode.B) && isJumping == true && PlayerMovement.instance.isGrounded)
        {
            if (jumpCounter > 0)
            {
                rb2d.velocity = Vector2.up * jumpPower;
                jumpCounter -= Time.deltaTime;
                animator.Play("player_jump");
            }

            else
            {
                isJumping = false;
            }

        }
        if (Input.GetKeyUp(KeyCode.B) && PlayerMovement.instance.isGrounded)
        {
            isJumping = false;
            animator.Play("player_jump");
        }
        animator.SetBool("isJumping", isJumping);
    }
}
