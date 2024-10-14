using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   [SerializeField] private float camSpeeed;
    private float postX;
    private Vector3 velocity = Vector3.zero;

    //Camera follow Player
    [SerializeField] private Transform Player;
    public float camDistance;
    private float facingDirection;
   

    private void Update()
    {
        // Camera follow the scene
       // transform.position = Vector3.SmoothDamp(transform.position, new Vector3(postX, transform.position.y,transform.position.z),ref velocity,camSpeeed);
       transform.position = new Vector3(Player.position.x,Player.position.y,transform.position.z);
        transform.position = new Vector3(Player.position.x + facingDirection, Player.position.y, transform.position.z);
        facingDirection = Mathf.Lerp(facingDirection,(camDistance * Player.localScale.x),Time.deltaTime * camSpeeed);
     
    }
    public void MoveToNewScene(Transform newScene)
    {
        postX = newScene.position.x;
      
    }

}
