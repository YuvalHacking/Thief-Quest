using UnityEngine;
using System;

[System.Serializable]
public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public float startingHealth;  // The initial health value of the object

    [Header("Components")]
    [SerializeField] private Behaviour[] components;  // Array to hold other component scripts to be disabled


    public static event Action onPlayerDeath;
    public float currentHealth;  // The current health of the object
    private Animator anim;  // Reference to the Animator component
    private bool dead;  // Flag to track if the object is dead


    private void Awake()
    {
        currentHealth = startingHealth;  // Set the current health to the starting health
        anim = GetComponent<Animator>();  // Get the Animator component of the object
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
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);  // Deactivate the entire GameObject
    }
}
