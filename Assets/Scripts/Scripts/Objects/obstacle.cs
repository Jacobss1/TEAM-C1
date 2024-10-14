using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    [SerializeField] int damage = 1;

    public AudioSource audioSource;
    public AudioClip damageSound;



    private void Start()
    {
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth playerHealth = collision.gameObject.GetComponent<playerHealth>();
            if (playerHealth != null)
            {
                // Pass the obstacle's position to calculate knockback direction
                playerHealth.TakeDamage(damage, transform.position);
            }
        }
        PlayDamageSound();
    }

    public void PlayDamageSound()
    {


    }
}
