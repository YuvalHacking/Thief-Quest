using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Method to start the game (loaded the next scene)
    public void StartGame()
    {
        // Load the next scene based on the current build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Method to quit the application
    public void Quit()
    {
        Application.Quit(); // Close the application
    }

    // Method to go back to the start menu scene
    public void BackToStart()
    {
        // Load the scene with build index 0 (start menu scene)
        SceneManager.LoadScene(0);
    }

    // Method to retry the current level
    public void Retry()
    {
        // Load the scene with build index 1 (the current level)
        SceneManager.LoadScene(1);
    }
}
