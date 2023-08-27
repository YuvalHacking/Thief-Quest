using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SerializationManager : MonoBehaviour
{
    // Reference to the player's Health component
    Health playerHealth;

    private void Awake()
    {
        // Get the player's Health component
        playerHealth = GetComponent<Health>();
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        // Check if the current scene is the one where serialization is needed
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            LoadFromJson(); // Load data from JSON
        }
    }

    public void SaveToJson()
    {
        // Create a Health object to store data
        Health data = new Health();

        // Populate the data with player's health values
        data.startingHealth = playerHealth.startingHealth;
        data.currentHealth = playerHealth.currentHealth;

        // Convert the data to JSON format
        string json = JsonUtility.ToJson(data, true);

        // Write the JSON data to a file
        File.WriteAllText(Application.dataPath + "/Serialization/PlayerHealth.json", json);
    }

    public void LoadFromJson()
    {
        // Read the JSON data from the file
        string json = File.ReadAllText(Application.dataPath + "/Serialization/PlayerHealth.json");

        // Deserialize the JSON data into a HealthSerilazable object
        HealthSerilazable data = JsonUtility.FromJson<HealthSerilazable>(json);

        // Update the player's health values with the loaded data
        playerHealth.currentHealth = data.currentHealth;
        playerHealth.startingHealth = data.startingHealth;
    }
}