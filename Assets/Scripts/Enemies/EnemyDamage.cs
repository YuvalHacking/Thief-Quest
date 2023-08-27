using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // Damage value for the enemy's attack
    [SerializeField] protected float damage;

    // Called when a collider enters the trigger zone of the enemy
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.tag == "Player")
            // Damage the player by invoking the TakeDamage() method from their Health component
            collision.GetComponent<Health>().TakeDamage(damage);
    }
}
