using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player1;
    [SerializeField] float timeOffset = 0.3f;
    [SerializeField] Vector3 posOffset = new Vector3(0, 0, -10); // Ensure the camera z position is correct
    [SerializeField] float leftLimit;
    [SerializeField] float rightLimit;
    [SerializeField] float bottomLimit;
    [SerializeField] float topLimit;
    private Vector3 velocity;

    private void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player1 == null)
        {
            player1 = GameObject.FindGameObjectWithTag("Player");
        }
        if (player1 != null)
        {
            CameraFollowToPlayerWithCornerBoundary();
        }
    }

    void CameraFollowToPlayerWithCornerBoundary()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = player1.transform.position + posOffset; // Apply offset directly
        transform.position = Vector3.SmoothDamp(startPos, endPos, ref velocity, timeOffset);

        // Clamp camera position within limits
        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
            Mathf.Clamp(transform.position.y, bottomLimit, topLimit),
            transform.position.z
        );
    }
}
