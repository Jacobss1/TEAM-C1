using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallFaster : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float fallMultiplier;
    public float upMultiplier;

    void Start()
    {

    }

    void Update()
    {
        if (rb2d.velocity.y > 0) // make fall down faster
        {
            rb2d.velocity += Vector2.up * Physics.gravity.y * (upMultiplier) * Time.deltaTime;
        }

        if (rb2d.velocity.y < 0) // make fall down faster
        {
            rb2d.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier) * Time.deltaTime;
        }
    }
}
