using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float jumpBoost;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb2d = collision.gameObject.GetComponent<Rigidbody2D>();
            Animator animator = collision.gameObject.GetComponent<Animator>();

            if (rb2d != null)
            {
                // Reset the player's vertical velocity before applying the jump boost
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);

                // Apply an upward force to simulate a trampoline bounce
                rb2d.AddForce(Vector2.up * jumpBoost, ForceMode2D.Impulse);

                // Play the mid air animation
                if (animator != null)
                {
                    animator.Play("mid-air");
                }
            }
        }
    }
}
