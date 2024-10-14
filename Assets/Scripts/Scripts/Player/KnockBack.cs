using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float knockbackTime = 0.3f;     // Duration of the knockback
    public float hitDirectionForce = 10f;  // Horizontal force applied on knockback
    public float verticalForce = 5f;       // Vertical force applied on knockback
    public float inputForce = 7.5f;        // How much the player's input affects the knockback
    private Rigidbody2D rb2d;
    private Coroutine knockBackCoroutine;
    playerHealth health;

    public bool isBeingKnockBack { get; private set; }

    private Animator anim; // Reference to the Animator

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Initialize Animator reference
    }

    private IEnumerator KnockBackAction(Vector2 hitDirection, float inputDirection)
    {
   
            isBeingKnockBack = true;

            // Play the knockback animation
            anim.SetTrigger("KnockBack");

            Vector2 knockbackForce;
            float elapsedTime = 0f;

            // Add some upward force to the knockback to simulate a realistic hit
            Vector2 upwardForce = new Vector2(0f, verticalForce);

            while (elapsedTime < knockbackTime)
            {
                elapsedTime += Time.fixedDeltaTime;

                // Combine hit direction force and upward force to give a realistic knockback effect
                knockbackForce = hitDirection * hitDirectionForce + upwardForce;

                // Add player's input force if they're pressing any direction
                if (inputDirection != 0)
                {
                    knockbackForce += new Vector2(inputDirection * inputForce, 0f);
                }

                // Apply the knockback force to the player's velocity
                rb2d.velocity = knockbackForce;

                yield return new WaitForFixedUpdate();
            }
       
        
            // Smoothly stop the knockback after the time has elapsed
            isBeingKnockBack = false;
            rb2d.velocity = Vector2.zero;  // Reset velocity after knockback ends

            // Reset to the idle or default state after knockback
            anim.SetBool("isGrounded", true);


      

    }

    public void callKnockBack(Vector2 hitDirection, float inputDirection)
    {
        if (knockBackCoroutine != null)
        {
            StopCoroutine(knockBackCoroutine);  // Stop any existing knockback coroutine
        }

        knockBackCoroutine = StartCoroutine(KnockBackAction(hitDirection, inputDirection));
    }
}
