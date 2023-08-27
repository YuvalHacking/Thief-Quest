using UnityEngine;
using System;

// This class is used to store health-related values that can be serialized
[System.Serializable]
public class HealthSerilazable
{
    // The initial health value of the object
    public float startingHealth;

    // The current health of the object
    public float currentHealth;
}
