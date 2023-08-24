using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyToPlayer : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private int viewRange;
    [SerializeField] private float attackRangeX;
    [SerializeField] private float attackRangeY;


    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistanceAttack;
    [SerializeField] private float colliderDistanceView;
    [SerializeField] private BoxCollider2D boxCollider;


    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask playerProjecileLayer;


    private UnityEngine.AI.NavMeshAgent agent;
    private float cooldownTimer = Mathf.Infinity;
    private bool isFoundPlayer;
    private Health playerHealth;



    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if ((PlayerInSight() || isFoundPlayer))
        {
            agent.destination = target.position;
            isFoundPlayer = true;
            CheckFlip();
        }

        cooldownTimer += Time.deltaTime;

        if (PlayerInAttackRange() && (cooldownTimer >= attackCooldown))
        {
            cooldownTimer = 0;
            playerHealth.TakeDamage(damage);
        }
    }

    private bool PlayerInAttackRange()
    {
        RaycastHit2D hit =
             Physics2D.BoxCast(boxCollider.bounds.center + transform.right * attackRangeX * transform.localScale.x * colliderDistanceAttack,
             new Vector3(boxCollider.bounds.size.x * attackRangeX, boxCollider.bounds.size.y * attackRangeY, boxCollider.bounds.size.z),
             0, Vector2.left, 0, playerLayer);


        return hit.collider != null;
    }

    private bool PlayerInSight()
    {
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
        //attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * attackRangeX * transform.localScale.x * colliderDistanceAttack,
           new Vector3(boxCollider.bounds.size.x * attackRangeX, boxCollider.bounds.size.y * attackRangeY, boxCollider.bounds.size.z));

        //view
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