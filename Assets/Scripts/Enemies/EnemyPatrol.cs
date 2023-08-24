using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float DetectSpeed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    [Header("Player")]
    [SerializeField] private GameObject player;

    public bool isPatrol = true;

    private void Awake()
    {
        initScale = enemy.localScale;
        movingLeft = initScale.x <= 0;
    }
    private void OnDisable()
    {
        anim.SetBool("Walk", false);
    }

    private void Update()
    {
        if (isPatrol)
        {

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
            MoveToPlayer();
        }
    }

    private void DirectionChange()
    {
        if (isPatrol)
        {
            anim.SetBool("Walk", false);
            idleTimer += Time.deltaTime;
        }

        if (idleTimer > idleDuration || !isPatrol)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction, float newSpeed)
    {
        idleTimer = 0;
        anim.SetBool("Walk", true);

        //Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * newSpeed,
            enemy.position.y, enemy.position.z);
    }

    public void MoveToPlayer()
    {//check when ranged?

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