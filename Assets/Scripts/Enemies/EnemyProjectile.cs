using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    // Projectile speed and reset time
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;

    // Variables to track projectile lifetime and animations
    private float lifetime;
    private Animator anim;
    private BoxCollider2D coll;

    // Flag to indicate if the projectile has hit something
    private bool hit;

    private void Awake()
    {
        // Initialize animator and collider references
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Activate the projectile and reset its properties
    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }

    private void Update()
    {
        // Skip update if the projectile has hit something
        if (hit) return;

        // Move the projectile forward based on its speed
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        // Increment the lifetime of the projectile
        lifetime += Time.deltaTime;

        // Deactivate the projectile if its lifetime exceeds the reset time
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for collision with specific tags
        if (collision.tag != "Ladder" && collision.tag != "Throwing_Star" && collision.tag != "Enemy")
        {
            // Projectile has hit something, perform appropriate actions

            hit = true;
            base.OnTriggerEnter2D(collision); // Execute logic from parent script first
            coll.enabled = false;

            if (anim != null)
                anim.SetTrigger("Explode"); // When the object is a fireball, trigger explosion animation
            else
                gameObject.SetActive(false); // When this hits any object, deactivate the projectile
        }
    }

    // Deactivate the projectile
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
