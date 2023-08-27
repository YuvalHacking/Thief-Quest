using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;

        // Calculate movement distance based on speed and direction
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        // Track projectile lifetime
        lifetime += Time.deltaTime;
        if (lifetime > 3) gameObject.SetActive(false); // Deactivate projectile after a certain lifetime
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Ladder" && collision.tag != "Fireball")
        {
            hit = true;
            boxCollider.enabled = false;
            gameObject.SetActive(false);
        }

        // Damage enemy if collided
        if (collision.tag == "Enemy")
            collision.GetComponent<Health>().TakeDamage(1);
    }

    // Set the direction of the projectile
    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = -_direction; // Reverse the direction
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        // Adjust the local scale based on the direction to flip the projectile
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    // Deactivate the projectile
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
