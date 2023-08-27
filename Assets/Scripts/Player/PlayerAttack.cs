using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject[] throwingStars;

    private Animator animator;
    private Player player;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        // Check for attack input and cooldown
        if (Input.GetButton("Fire1") && cooldownTimer > attackCooldown && player.CanAttack())
        {
            Attack();
        }

        cooldownTimer += Time.deltaTime;

        // Check for attack input during jump and cooldown
        if (Input.GetButton("Fire1") && cooldownTimer > attackCooldown && player.CanAttack() && animator.GetBool("IsJumping"))
        {
            Attack();
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        cooldownTimer = 0;

        int throwingStarIndex = FindThrowingStar(); // Find an inactive throwing star
        if (throwingStarIndex != -1)
        {
            throwingStars[throwingStarIndex].transform.position = throwPoint.position;
            throwingStars[throwingStarIndex].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }

    private int FindThrowingStar()
    {
        for (int i = 0; i < throwingStars.Length; i++)
        {
            if (!throwingStars[i].activeInHierarchy)
                return i;
        }
        return -1; // No inactive throwing stars found
    }
}
