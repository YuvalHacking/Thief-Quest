using UnityEngine;

public class AlienAttack : MonoBehaviour
{
    // Attack Parameters
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float hitCoolDown;
    [SerializeField] private float attackRange;
    [SerializeField] private int damage;
    [SerializeField] private int viewRange;

    // Ranged Attack
    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;

    // Collider Parameters
    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistanceAttack;
    [SerializeField] private float colliderDistanceView;
    [SerializeField] private BoxCollider2D boxCollider;

    // Player Layers
    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask playerProjecileLayer;

    // Cooldown Timers
    private float cooldownTimer = Mathf.Infinity;
    private float cooldownTimerCollisionHit = Mathf.Infinity;

    // References
    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        // Initialize references
        anim = GetComponent<Animator>();
        enemyPatrol = transform.parent.parent.Find("Alien_Holder").gameObject.GetComponentInChildren<EnemyPatrol>();
    }

    private void Update()
    {
        // Check if the player is in sight to adjust patrol behavior
        if (PlayerInSight())
        {
            enemyPatrol.isPatrol = false;
        }
        else
        {
            enemyPatrol.isPatrol = true;
        }

        // Attack logic
        cooldownTimer += Time.deltaTime;
        cooldownTimerCollisionHit += Time.deltaTime;

        if (PlayerInAttackRange())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("Attack");
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInAttackRange();
    }

    private bool PlayerInAttackRange()
    {
        // Check if the player is in attack range
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistanceAttack,
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private bool PlayerInSight()
    {
        // Check if the player is in sight range
        RaycastHit2D viewPlayer =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * viewRange * transform.localScale.x * colliderDistanceView,
            new Vector3(boxCollider.bounds.size.x * viewRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        RaycastHit2D viewPlayerProjectile =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * viewRange * transform.localScale.x * colliderDistanceView,
            new Vector3(boxCollider.bounds.size.x * viewRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerProjecileLayer);

        if (viewPlayer.collider != null)
            playerHealth = viewPlayer.transform.GetComponent<Health>();

        return (viewPlayer.collider != null || viewPlayerProjectile.collider != null);
    }

    private void OnDrawGizmos()
    {
        // Draw gizmos to visualize attack and view ranges
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistanceAttack,
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * viewRange * transform.localScale.x * colliderDistanceView,
            new Vector3(boxCollider.bounds.size.x * viewRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        // Damage the player when in attack range
        if (PlayerInAttackRange())
            playerHealth.TakeDamage(damage);
    }

    private void RangedAttack()
    {
        // Perform ranged attack
        cooldownTimer = 0;
        fireballs[FindFireball()].transform.position = firepoint.position;
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireball()
    {
        // Find an available fireball for ranged attack
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage the player on collision
        if (playerHealth != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (cooldownTimerCollisionHit >= hitCoolDown)
                {
                    cooldownTimerCollisionHit = 0;
                    playerHealth.TakeDamage(1);
                }
            }
        }
    }
}
