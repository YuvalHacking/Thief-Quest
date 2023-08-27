using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    // Patrol Points
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    // Enemy
    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    // Movement parameters
    [Header("Movement parameters")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float DetectSpeed;
    private Vector3 initScale;
    private bool movingLeft;

    // Idle Behaviour
    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    // Enemy Animator
    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    // Player
    [Header("Player")]
    [SerializeField] private GameObject player;

    // Flag to control whether the enemy should patrol or move to the player
    public bool isPatrol = true;

    private void Awake()
    {
        // Store the initial scale and determine initial movement direction
        initScale = enemy.localScale;
        movingLeft = initScale.x <= 0;
    }

    private void OnDisable()
    {
        // Turn off the walk animation when the enemy is disabled
        anim.SetBool("Walk", false);
    }

    private void Update()
    {
        if (isPatrol)
        {
            // Patrol logic
            if (movingLeft)
            {
                if (enemy.position.x >= leftEdge.position.x)
                    MoveInDirection(-1, patrolSpeed);
                else
                    DirectionChange();
            }
            else
            {
                if (enemy.position.x <= rightEdge.position.x)
                    MoveInDirection(1, patrolSpeed);
                else
                    DirectionChange();
            }
        }
        else
        {
            // Move-to-player logic
            MoveToPlayer();
        }
    }

    private void DirectionChange()
    {
        if (isPatrol)
        {
            // Transition to idle state and start counting idle time
            anim.SetBool("Walk", false);
            idleTimer += Time.deltaTime;
        }

        // Switch direction if idle duration is reached or isPatrol flag is off
        if (idleTimer > idleDuration || !isPatrol)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction, float newSpeed)
    {
        // Reset idle timer and enable walking animation
        idleTimer = 0;
        anim.SetBool("Walk", true);

        // Update enemy's scale to face the correct direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        // Move the enemy in the specified direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * newSpeed,
            enemy.position.y, enemy.position.z);
    }

    public void MoveToPlayer()
    {
        // Activate walking animation
        anim.SetBool("Walk", true);

        if (movingLeft)
        {
            if (enemy.position.x >= player.transform.position.x)
                MoveInDirection(-1, DetectSpeed);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= player.transform.position.x)
                MoveInDirection(1, DetectSpeed);
            else
                DirectionChange();
        }
    }
}
