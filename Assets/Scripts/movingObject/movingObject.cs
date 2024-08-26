using UnityEngine;

public class TriggerMoveObject2D : MonoBehaviour
{
    public GameObject MovingObject; // The object that will move
    public Transform targetPosition; // The position to move the object towards
    public float moveSpeed = 5f; // Speed at which the object moves
    public float rollingSpeed = 180f;
   // public Animator playerAnimator; // Animator component of the player

    private bool shouldMove = false;

    // This method is called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            shouldMove = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            // Move the object towards the target position
            MovingObject.transform.position = Vector2.MoveTowards(MovingObject.transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
            MovingObject.transform.Rotate(Vector3.forward * rollingSpeed * Time.deltaTime);
            // Optionally, stop the movement when the object reaches the target position
            if (Vector2.Distance(MovingObject.transform.position, targetPosition.position) < 0.1f)
            {
                shouldMove = false;
            }
        }
    }
}
