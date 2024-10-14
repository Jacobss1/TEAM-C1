using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public Transform pointA; // Patrol start point
    public Transform pointB; // Patrol end point
    public float patrolSpeed = 2f; // Speed when patrolling
    public float chaseSpeed = 5f; // Speed when chasing the player
    public float retreatDistance = 2f; // Distance to move back from the player after hitting
    public float retreatDuration = 1f; // Time to retreat before resuming chase
    public int damage = 1; // Damage dealt to the player

    private Transform player;
    private Vector2 currentTarget;
    private bool isChasing = false;
    private bool isRetreating = false;
    private bool isDisabled = false; // New flag to check if actions are disabled
    private Rigidbody2D rb;
    private Vector2 retreatPosition;
    private float retreatEndTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointA.position; // Set initial target
    }

    private void Update()
    {
        if (!isDisabled)
        {
            Move();
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop movement when disabled
        }
    }

    private void Move()
    {
        if (isRetreating)
        {
            // Move the enemy back from the player
            Vector2 direction = (retreatPosition - (Vector2)transform.position).normalized;
            rb.velocity = direction * patrolSpeed;

            if (Vector2.Distance(transform.position, retreatPosition) < 0.1f)
            {
                // Retreat phase completed, stop retreating
                isRetreating = false;
                retreatEndTime = Time.time + retreatDuration;
            }
        }
        else if (isChasing && Time.time > retreatEndTime)
        {
            // Chase the player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * chaseSpeed;

            // Check if the enemy is close enough to hit the player
            if (Vector2.Distance(transform.position, player.position) < 0.5f)
            {
                // Damage the player
                playerHealth playerHealth = player.GetComponent<playerHealth>();
                if (playerHealth != null)
                {
                   // playerHealth.TakeDamage(damage);
                }

                // Start retreat phase
                retreatPosition = (Vector2)transform.position - (Vector2)(direction * retreatDistance);
                isRetreating = true;
            }
        }
        else if (!isChasing && !isRetreating)
        {
            // Patrol between points
            Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;
            rb.velocity = direction * patrolSpeed;

            if (Vector2.Distance(transform.position, currentTarget) < 0.1f)
            {
                // Switch targets
                currentTarget = currentTarget == (Vector2)pointA.position ? (Vector2)pointB.position : (Vector2)pointA.position;
            }
        }
    }

    public void DisableActions()
    {
        isDisabled = true; // Disable enemy actions
    }

    public void EnableActions()
    {
        isDisabled = false; // Enable enemy actions
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isChasing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth playerHealth = collision.gameObject.GetComponent<playerHealth>();
            if (playerHealth != null)
            {
              //  playerHealth.TakeDamage(damage);
            }

            // Start retreat phase
            Vector2 direction = (player.position - transform.position).normalized;
            retreatPosition = (Vector2)transform.position - (Vector2)(direction * retreatDistance);
            isRetreating = true;
        }
    }
}
