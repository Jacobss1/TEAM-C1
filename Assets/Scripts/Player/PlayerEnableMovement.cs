using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnableMovement : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject playerGameObject;
   // public  PlayerJump playerJump;
    public Rigidbody2D rb2d;

    public void PlayerMovementENABLE()
    {
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        playerMovement.enabled = true;

      
    }
 /*   public void PlayerJumpENABLE()
    {
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        playerJump.enabled = true;
    }
 */
    public void DestroyFunciton()
    {
        // Destroy this GameObject including the main parent game object were this object is attached after 0.5 seconds
        playerGameObject.SetActive(false);
        //Destroy(gameObject.transform.root.gameObject, 0.5f);
    }

}
