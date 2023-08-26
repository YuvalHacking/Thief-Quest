using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{

    [SerializeField] private GameObject gameOverMenu;

    private void OnEnable()
    {
        Health.onPlayerDeath += EnableGameOverMenu;
    }

    private void OnDisable()
    {
        Health.onPlayerDeath -= EnableGameOverMenu;
    }

    public void EnableGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }
}