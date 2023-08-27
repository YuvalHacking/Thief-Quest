
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SerializationManager : MonoBehaviour
{
    Health playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();

        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
            LoadFromJson();
    }

    public void SaveToJson()
    {
        Health data = new Health();
        data.startingHealth = playerHealth.startingHealth;
        data.currentHealth = playerHealth.currentHealth;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/Serialization/PlayerHealth.json", json);
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/Serialization/PlayerHealth.json");
        HealthSerilazable data = JsonUtility.FromJson<HealthSerilazable>(json);
        Debug.Log(data.startingHealth);

        playerHealth.currentHealth = data.currentHealth;
        playerHealth.startingHealth = data.startingHealth;
    }
}