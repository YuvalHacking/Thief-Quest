using UnityEngine;

public class DragonDagger : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private int damage;
    [SerializeField] private int viewRange;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistanceAttack;
    [SerializeField] private float colliderDistanceView;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask playerProjecileLayer;
    
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = transform.parent.parent.Find("Dragon_Holder").gameObject.GetComponentInChildren<EnemyPatrol>();
    }

    private void Update()
    {
        if (PlayerInSight())
        {
            enemyPatrol.isPatrol = false;
        }
        else
        {
            enemyPatrol.isPatrol = true;
        }

        //atttack
        cooldownTimer += Time.deltaTime;

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
        //attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistanceAttack,
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        //view
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * viewRange * transform.localScale.x * colliderDistanceView,
            new Vector3(boxCollider.bounds.size.x * viewRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInAttackRange())
            playerHealth.TakeDamage(damage);
    }

}