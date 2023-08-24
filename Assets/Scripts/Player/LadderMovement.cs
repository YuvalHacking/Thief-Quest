using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private float vertical;
    private float speed = 8f;
    private bool isLadder;
    public bool isClimbing;

    [SerializeField] private Rigidbody2D rigidBody;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        if (isLadder)
        {
            if (Mathf.Abs(vertical) > 0f || Mathf.Abs(vertical) < 0f)
            {
                isClimbing = true;
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsClimbing", true);

                animator.speed = 1;

            }
            else
            {
                animator.speed = 0;
            }
        }

        if (isClimbing)
        {
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, vertical * speed);
        }
        else
        {
            rigidBody.gravityScale = 4f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
            animator.SetBool("IsClimbing", false);
            animator.speed = 1;
        }
    }
}
