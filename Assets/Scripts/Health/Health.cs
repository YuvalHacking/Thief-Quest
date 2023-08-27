using UnityEngine;
using System;

[System.Serializable]
public class Health : MonoBehaviour
{
    // The initial health value of the object
    [Header("Health")]
    [SerializeField] public float startingHealth;

    // Array to hold other component scripts to be disabled
    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    // Event to be triggered on player death
    public static event Action onPlayerDeath;

    // The current health of the object
    public float currentHealth;

    // Reference to the Animator component
    private Animator anim;

    // Flag to track if the object is dead
    private bool dead;

    private void Awake()
    {
        // Set the current health to the starting health
        currentHealth = startingHealth;

        // Get the Animator component of the object
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        // Reduce current health by the damage taken, clamped between 0 and starting health
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");  // Trigger the "Hurt" animation if health is still positive
        }
        else
        {
            if (!dead)  // Check if the object is not already dead
            {
                if (gameObject.tag == "Player")
                {
                    // If the object is tagged as "Player", reset certain animator parameters
                    anim.SetBool("IsJumping", false);
                    anim.SetBool("IsClimbing", false);

                    // Invoke the onPlayerDeath event
                    onPlayerDeath?.Invoke();
                }

                anim.SetBool("Walk", false);  // Set "IsWalking" animator parameter to false
                anim.SetTrigger("Death");  // Trigger the "Death" animation

                // Deactivate all attached component classes
                foreach (Behaviour component in components)
                    component.enabled = false;

                dead = true;  // Mark the object as dead
            }
        }
    }

    public void AddHealth(float value)
    {
        // Increase current health by the specified value, clamped between 0 and starting health
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }

    // Deactivate the entire GameObject
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}