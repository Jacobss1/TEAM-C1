using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingGround : MonoBehaviour
{
    public Transform postA, postB;
    public float objectSpeed;
    Vector2 targetPosition;

    void Start()
    {
        targetPosition = postB.position; // Start by moving towards postB
    }

    void Update()
    {
        // Debugging: Check current position and target position
        Debug.Log("Current Position: " + transform.position);
        Debug.Log("Target Position: " + targetPosition);

        // Move object between postA and postB
        if (Vector2.Distance(transform.position, postA.position) < .1f)
        {
            targetPosition = postB.position; // Switch to moving towards postB
            Debug.Log("Switching target to postB");
        }
        else if (Vector2.Distance(transform.position, postB.position) < .1f)
        {
            targetPosition = postA.position; // Switch to moving towards postA
            Debug.Log("Switching target to postA");
        }

        // Move the object towards the target
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, objectSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
