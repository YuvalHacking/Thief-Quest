using UnityEngine;

public class EnemyProjectileHolder : MonoBehaviour
{
    // Reference to the enemy transform
    [SerializeField] private Transform enemy;

    private void Update()
    {
        // Update the scale of the holder to match the enemy's scale but flipped horizontally
        transform.localScale = new Vector3(-enemy.localScale.x, enemy.localScale.y, enemy.localScale.z);
    }
}
