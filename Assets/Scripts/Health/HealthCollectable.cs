using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    // The amount of health to add when collected
    [SerializeField] private float healthValue;

    // Reference to the Health component of the player
    Health health;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the Health component from the collider
        health = collision.collider.GetComponent<Health>();

        // Check if the collided object has the "Player" tag
        if (collision.collider.CompareTag("Player"))
        {
            // Add health to the player using the AddHealth method from the Health component
            health.AddHealth(healthValue);

            // Deactivate the collectable object
            gameObject.SetActive(false);
        }
    }
}
