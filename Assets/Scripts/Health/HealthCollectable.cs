using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    [SerializeField] private float healthValue;
    Health health;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        health = collision.collider.GetComponent<Health>();
        if (collision.collider.CompareTag("Player") && health.currentHealth < health.startingHealth)
        {
            health.AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}

