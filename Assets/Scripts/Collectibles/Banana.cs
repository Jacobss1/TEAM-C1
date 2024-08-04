using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
  //  public float bananaAmount = 0.10f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           // PlayerStatusBar playerStatusBar = collision.GetComponent<PlayerStatusBar>();
         /*  if (playerStatusBar != null)
            {
                playerStatusBar.OnTriggerEnter2D(GetComponent<Collider2D>());
            }
         */
            Destroy(gameObject);
        }
    }
}
