using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyToPlayer : MonoBehaviour
{
    // Target to move towards
    [Header("Target")]
    [SerializeField] private Transform target;

    // Attack Parameters
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private int viewRange;
    [SerializeField] private float attackRangeX;
    [SerializeField] private float attackRangeY;

    // Collider Parameters
    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistanceAttack;
    [SerializeField] private float colliderDistanceView;
    [SerializeField] private BoxCollider2D boxCollider;

    // Player Layers
    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask playerProjecileLayer;

    // NavMeshAgent component for movement
    private UnityEngine.AI.NavMeshAgent agent;

    // Cooldown timer for attacks
    private float cooldownTimer = Mathf.Infinity;

    // Flag to track if the player is detected
    private bool isFoundPlayer;

    // Reference to the player's health
    private Health playerHealth;

    private void Awake()
    {
        // Get the NavMeshAgent component and disable automatic rotation and up-axis updates
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        // If the player is detected or already found, move towards the target
        if ((PlayerInSight() || isFoundPlayer))
        {
            agent.destination = target.position;
            isFoundPlayer = true;
            CheckFlip(); // Adjust flip direction based on movement
        }

        // Update the cooldown timer
        cooldownTimer += Time.deltaTime;

        // Attack the player if in attack range and cooldown is met
        if (PlayerInAttackRange() && (cooldownTimer >= attackCooldown))
        {
            cooldownTimer = 0;
            playerHealth.TakeDamage(damage);
        }
    }

    private bool PlayerInAttackRange()
    {
        // Check if the player is within attack range
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * attackRangeX * transform.localScale.x * colliderDistanceAttack,
            new Vector3(boxCollider.bounds.size.x * attackRangeX, boxCollider.bounds.size.y * attackRangeY, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private bool PlayerInSight()
    {
        // Check if the player is within view range
        RaycastHit2D viewPlayer =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * viewRange * transform.localScale.x * colliderDistanceView,
            new Vector3(boxCollider.bounds.size.x * viewRange, boxCollider.bounds.size.y * viewRange, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        RaycastHit2D viewPlayerProjectile =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * viewRange * transform.localScale.x * colliderDistanceView,
            new Vector3(boxCollider.bounds.size.x * viewRange, boxCollider.bounds.size.y * viewRange, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerProjecileLayer);

        if (viewPlayer.collider != null)
            playerHealth = viewPlayer.transform.GetComponent<Health>();

        return (viewPlayer.collider != null || viewPlayerProjectile.collider != null);
    }

    private void OnDrawGizmos()
    {
        // Draw Gizmos to visualize attack and view ranges
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * attackRangeX * transform.localScale.x * colliderDistanceAttack,
           new Vector3(boxCollider.bounds.size.x * attackRangeX, boxCollider.bounds.size.y * attackRangeY, boxCollider.bounds.size.z));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * viewRange * transform.localScale.x * colliderDistanceView,
            new Vector3(boxCollider.bounds.size.x * viewRange, boxCollider.bounds.size.y * viewRange, boxCollider.bounds.size.z));
    }

    private void CheckFlip()
    {
        bool isFacingRight = transform.localScale.x < 0;

        if (agent.desiredVelocity.x > 0 && !isFacingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Facing right
        }

        if (agent.desiredVelocity.x < 0 && isFacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1); // Facing left
        }
    }
}