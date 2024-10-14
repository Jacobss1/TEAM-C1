using UnityEngine;

public class Projectile : MonoBehaviour
{

    public int damage;
    [SerializeField] public float speed;
    private float direction;
    private bool hit;
    public float maxLifetime = 5f;  // Maximum lifetime for the projectile

    private Animator anim;
    private BoxCollider2D boxCollider;
    private float currentLifetime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;

        // Move the projectile
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        // Update the lifetime
        currentLifetime += Time.deltaTime;
        if (currentLifetime > maxLifetime)
            Deactivate();  // Deactivate after exceeding max lifetime

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;


        enemyHealth enemy = collision.GetComponent<enemyHealth>();
        if(enemy != null)
        {
            enemy.TakeDamage(damage, transform.position);
        }
        // Play hit animation, if any
        if (anim != null)
        {
            anim.SetTrigger("explode");
        }

        // Deactivate the projectile after a short delay (to let the animation play, if any)
        Invoke("Deactivate", 0.5f);  // Adjust the delay to match your explosion animation length
    }

    public void SetDirection(float _direction)
    {
        // Reset the projectile's lifetime and state when reused
        currentLifetime = 0;
        direction = _direction;
        hit = false;
        gameObject.SetActive(true);
        boxCollider.enabled = true;

        // Set the correct direction by flipping the projectile's scale if needed
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        // Deactivate the game object (return it to the object pool)
        gameObject.SetActive(false);
    }
}
