using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask jumpableGround;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private LadderMovement ladder;
    private PlayerAttack playerAttack;
    private bool isPortal;
    SerializationManager serializationManager;

    private void Awake()
    {
        ladder = GetComponent<LadderMovement>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerAttack = GetComponent<PlayerAttack>();
        serializationManager = GetComponent<SerializationManager>();
    }

    private void Update()
    {
        UserInput();
    }

    private void UserInput()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) && isPortal)
            MovePortal();

        if (IsGrounded() && Input.GetButtonDown("Jump") && !ladder.isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }


        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput, 0);

        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);

        // Flip the character's sprite based on movement direction
        if (movement.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Facing right
        }
        else if (movement.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Facing left
        }

        // Update the current state based on movement

        if (!ladder.isClimbing)
        {
            if (Mathf.Abs(rb.velocity.x) > 0.1f)
            {
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsJumping", false);
                // Running;
            }
            else
            {
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsJumping", false);
                //Idle;
            }

            if (!IsGrounded())
            {
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsJumping", true);
                //Jumping;
            }
        }

    }


    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    public bool IsJumping()
    {
        return !IsGrounded();
    }

    private void MovePortal()
    {
        serializationManager.SaveToJson();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public bool CanAttack()
    {
        return (!ladder.isClimbing && IsGrounded());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
            isPortal = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            isPortal = false;
        }
    }
}