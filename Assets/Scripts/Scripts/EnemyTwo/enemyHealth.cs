using System.Collections;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public int maxHealth = 2;
    public int currentHealth;
    Animator anim;
    public enemyhealhBar health; // Reference to health bar
    private KnockBack knockBack; // Reference to the KnockBack component
    private Rigidbody2D rb;
    Chase chase;
    EnemyPatrol patrol;

    // Particle effect prefab for hit
    [SerializeField] private GameObject hitEffectPrefab;

    // Reference to the player's Trail Renderer
    private TrailRenderer trailRenderer;

    // Enemy death status
    private bool isDying = false;

    // SFX
    public AudioSource audioSource;
    public AudioClip hurtSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        health.SetMaxHealth(maxHealth); // Initialize the health bar
        knockBack = GetComponent<KnockBack>(); // Initialize the KnockBack reference

        // Get the TrailRenderer component (if any)
        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false; // Disable trail by default
        }
    }

    public void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        // Deduct health and update the health bar
        currentHealth -= damage;
        health.SetHealth(currentHealth); // Update the health bar

        // Play hit particle effect
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1f); // Destroy particle effect after 1 second
        }

        // Enable trail when taking damage
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
            StartCoroutine(DisableTrailAfterDelay(0.5f)); // Disable trail after 0.5 seconds
        }

        // If the enemy is still alive, apply knockback
        if (currentHealth > 0)
        {
            Vector2 hitDirection = (transform.position - new Vector3(damageSourcePosition.x, damageSourcePosition.y, 0)).normalized;
            knockBack.callKnockBack(hitDirection, 0); // Adjust inputDirection as needed
        }
        else if (currentHealth <= 0 && !isDying) // If the enemy dies
        {
            isDying = true;
            // Die();
            
            anim.SetTrigger("enemyDead"); // Trigger death animation
         
            // Handle death after animation
        }


        PlaySound(); // Play hurt sound
    }

    private IEnumerator DisableTrailAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
    }

  

    public void Die()
    {

        gameObject.SetActive(false); // Deactivate the enemy object

        Debug.Log("Enemy has died.");
    }

    public void PlaySound()
    {
        if (hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
    }
    public void DisableFunc()
    {
        patrol.enabled = false;
        chase.enabled = false;
    }
}
