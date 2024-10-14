using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClassContainer : MonoBehaviour
{

    public KeyCode crouchKey = KeyCode.C; // Key to crouch
    public float crouchSpeed = 2f; // Speed when crouching
    public bool isCrouching = false;
    public bool isJumping = false; // Flag to track if the player is jumping

    public Vector2 originalColliderSize;
    public Vector2 originalColliderOffset;
    public Vector2 slideColliderSize;
    public Vector2 slideColliderOffset;

    // Add a buffer time to prevent the grounded state from being interrupted
    public float groundedBufferTime = 0.1f;
    public float groundedBufferCounter = 0f;
    public bool isGroundCheckBuffered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
