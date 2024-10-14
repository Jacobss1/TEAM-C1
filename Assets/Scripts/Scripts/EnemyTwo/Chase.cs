using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    [SerializeField] public Transform Player;          // Reference to the player
    public float chaseSpeed = 4f;     // Speed while chasing
    public float chaseRange = 5f;     // Distance to start chasing the player
    public float stopChaseRange = 7f; // Distance to stop chasing the player
    public float acceleration;
    public float minimumChaseSpeed;
    public float maximumChaseSpeed;

    private bool isChasing = false;   // Whether the enemy is in chase mode
    public SpriteRenderer spriteRenderer; // Reference to the sprite renderer
     EnemyPatrol patrol;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

   public void Update()
    {
        if (Player == null || !Player.gameObject.activeSelf)
        {
            isChasing = false;
            patrol.Patrol();
            // return;  //Exit the function early if no chase is possible
        }
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
      
        if (isChasing)
        { 
            if (distanceToPlayer > stopChaseRange)
            {
                isChasing = false;
            }
            else
            {
                FacePlayer();
                ChasePlayer();
            }
        }
        else
        {
            if (distanceToPlayer < chaseRange)
            {
                isChasing = true;
            }
        }
    }

     public void ChasePlayer()
    {
        Vector3 directionToPlayer = Player.position - transform.position;
        directionToPlayer.Normalize();

        chaseSpeed += acceleration * Time.deltaTime;
        chaseSpeed = Mathf.Clamp(chaseSpeed, minimumChaseSpeed, maximumChaseSpeed);

        transform.position += directionToPlayer * chaseSpeed * Time.deltaTime;
    }

   public  void FacePlayer()
    {
        // Enemy faces left by default, flip to face player when needed
        if (Player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;  // Face left (default)
        }
        else
        {
            spriteRenderer.flipX = true;   // Face right
        }
    }
}
