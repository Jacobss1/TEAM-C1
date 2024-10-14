using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class door : MonoBehaviour
{
    [SerializeField] private Transform previousScene;
    [SerializeField] private Transform currentScene;
      [SerializeField] CameraFollow camFollow;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(collision.transform.position.x < transform.position.x)
            {
                camFollow.MoveToNewScene(currentScene);
            }
            else
            {
                camFollow.MoveToNewScene(previousScene);
            }
        }
        {

        }
    }


}
