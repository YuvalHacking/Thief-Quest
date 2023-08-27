using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    // Reference to the player's Health component
    [SerializeField] private Health playerHealth;

    // Reference to the Image representing the total health bar
    [SerializeField] private Image totalHealthBar;

    // Reference to the Image representing the current health bar
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        // Set the initial fill amount of the total health bar based on the player's starting health
        totalHealthBar.fillAmount = playerHealth.startingHealth / 10;
    }

    private void Update()
    {
        // Update the fill amount of the current health bar based on the player's current health
        currentHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
