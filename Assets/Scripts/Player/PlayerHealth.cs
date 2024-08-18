using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image healthbar;
    public float healthAmount = 100f;
    public Rigidbody2D rb2d;
    public Animator animator;
    public GameObject playerVulnerabilityHitbox;
    [SerializeField] public PlayerMovement playerMovement;
  



    private bool isDead = false;
   // public bool isImmune = false; // Made public to access from DamageSpike

    void Start()
    {
        // Initialize the health bar
        healthbar.fillAmount = healthAmount / 100f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            Regen(5);
        }

        CheckIfDead(); // Check if the player is dead in every update
    }

    public void CheckIfDead()
    {
        if (healthAmount <= 0 && !isDead)
        {
            isDead = true;
            rb2d.bodyType = RigidbodyType2D.Static; // Changed to Static to stop all movement
            playerVulnerabilityHitbox.SetActive(false);
            playerMovement.enabled = false;
            Destroy(gameObject);
            //animator.Play("player_died"); // Use SetTrigger to play the death animation
        }
    }

   public void AttackDamage(int attackHit)
    {
        if (isDead) return; // Check if player is dead or immune

        healthAmount -= attackHit;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        healthbar.fillAmount = healthAmount / 100f;
    }

    public void Regen(float healingAmount)
    {
        if (isDead) return;

        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        healthbar.fillAmount = healthAmount / 100f;
    }

 /*   public void SetImmune(bool immune)
    {
        isImmune = immune;
    }
 */
}
