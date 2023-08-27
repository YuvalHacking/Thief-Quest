using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    // Reference to the game over menu GameObject
    [SerializeField] private GameObject gameOverMenu;

    // Called when this script is enabled
    private void OnEnable()
    {
        // Subscribe the EnableGameOverMenu method to the onPlayerDeath event
        Health.onPlayerDeath += EnableGameOverMenu;
    }

    // Called when this script is disabled
    private void OnDisable()
    {
        // Unsubscribe the EnableGameOverMenu method from the onPlayerDeath event
        Health.onPlayerDeath -= EnableGameOverMenu;
    }

    // Method to enable the game over menu
    public void EnableGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }
}