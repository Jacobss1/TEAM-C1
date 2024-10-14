using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA, pointB;        // Patrol points
    public float patrolSpeed = 2f;          // Speed while patrolling
    public SpriteRenderer spriteRenderer;   // Reference to the enemy's SpriteRenderer
    public float flipTolerance = 0.1f;      // Tolerance for flipping direction (smaller = more frequent flips)
    private Animator anim;                  // Optional Animator reference (if applicable)

    private Vector3 currentTarget;          // The current target position for patrolling

    void Start()
    {
        currentTarget = pointA.position;

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer assigned to EnemyPatrol. Please assign the SpriteRenderer component!");
        }
    }

    void Update()
    {
        Patrol();

        // Play idle animation if applicable
        if (anim != null)
        {
            anim.Play("enemy_idle");
        }
    }

    public void Patrol()
    {
        // Move towards the current patrol target
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, patrolSpeed * Time.deltaTime);

        // Calculate the direction to the current target
        Vector3 directionToTarget = currentTarget - transform.position;

        // Enemy faces left by default, flip only if moving right
        if (Mathf.Abs(directionToTarget.x) < flipTolerance)
        {
            spriteRenderer.flipX = directionToTarget.x < 0;  // Face right if moving right, otherwise left
        }

        // If the enemy reaches the current target, switch to the other point
        if (Vector3.Distance(transform.position, currentTarget) < 0.2f)
        {
            currentTarget = currentTarget == pointA.position ? pointB.position : pointA.position;
        }
    }
}
