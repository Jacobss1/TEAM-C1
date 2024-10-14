using System.Collections;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    Animator anim;
    public HealthBar healthBar;
    private KnockBack knockBack; // Reference to the KnockBack component
    private Rigidbody2D rb;

    // Reference to the particle effect prefab
    [SerializeField] private GameObject hitEffectPrefab;

    // Reference to the camera's StressReceiver
    public StressReceiver cameraStressReceiver;

    // Reference to the player's Trail Renderer
    private TrailRenderer trailRenderer;

    // Player death
    private bool isDying = false; // Initially set to false

    // SFX
    public AudioSource audioSource;
    public AudioClip hurtSound;

    private void Start()
    {
        rb = Container.Instance.rb;
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth); // Initialize the health bar
        knockBack = GetComponent<KnockBack>(); // Initialize the KnockBack reference

        // Find the camera's StressReceiver (assuming the main camera has this component)
        cameraStressReceiver = Camera.main.GetComponent<StressReceiver>();

        // Get the TrailRenderer component attached to the player (if any)
        trailRenderer = GetComponent<TrailRenderer>();

        if (trailRenderer != null)
        {
            trailRenderer.enabled = false; // Disable by default
        }
    }

    public void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        // Deduct health and update the health bar
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        // Instantiate hit particle effect
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
            effect.transform.SetParent(transform);
            Destroy(effect, 1f); // Adjust duration as needed
        }

        // Enable trail when taking damage
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
            StartCoroutine(DisableTrailAfterDelay(0.5f)); // Disable after 0.5 seconds
        }

        // Calculate hit direction
        Vector2 hitDirection = (transform.position - new Vector3(damageSourcePosition.x, damageSourcePosition.y, 0)).normalized;
        float inputDirection = Input.GetAxis("Horizontal");

        // If the player still has health, apply knockback
        if (currentHealth > 0)
        {
            knockBack.callKnockBack(hitDirection, inputDirection);
        }
        else if (currentHealth <= 0 && !isDying) // Handle death when health reaches 0
        {
            isDying = true; // Mark the player as dying
            // Set death animation and flag
           anim.SetTrigger("isDead");
           // Die();
          //  StartCoroutine(PlaydeathAnim());

          
        
              // Call death function
            
        }

        // Camera shake effect
        if (cameraStressReceiver != null)
        {
            cameraStressReceiver.InduceStress(0.3f);
        }

        // Play hurt sound
        PlaySound();
    }

    private IEnumerator DisableTrailAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
    }


   /* public IEnumerator PlaydeathAnim()
    {
        anim.SetTrigger("isDead");

        // Wait until the specific animation finishes playing
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
    }

    */
    public void Die()
    {
       
        
        gameObject.SetActive(false); // Deactivate the player object
        Debug.Log("Player has died.");
    }

    public void PlaySound()
    {
        if (hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
    }
}
