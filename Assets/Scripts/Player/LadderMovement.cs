using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private Player player; // Reference to the Player script
    private Animator animator; // Reference to the Animator component
    private float vertical; // Vertical input axis value
    private float speed = 8f; // Climbing speed
    private bool isLadder; // Flag to indicate if player is on a ladder
    public bool isClimbing; // Flag to indicate if player is currently climbing

    [SerializeField] private Rigidbody2D rigidBody; // Reference to the Rigidbody2D component

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        player = GetComponent<Player>(); // Get the Player script
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical"); // Get the vertical input axis value

        if (isLadder)
        {
            if (Mathf.Abs(vertical) > 0f || Mathf.Abs(vertical) < 0f)
            {
                isClimbing = true; // Set the climbing flag to true
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsClimbing", true); // Set the "IsClimbing" parameter in the animator

                animator.speed = 1; // Set the animator speed to 1 to play climbing animation

            }
            else
            {
                animator.speed = 0; // Set the animator speed to 0 to pause climbing animation
            }
        }

        if (isClimbing)
        {
            rigidBody.gravityScale = 0f; // Turn off gravity while climbing
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, vertical * speed); // Set vertical movement velocity
        }
        else
        {
            rigidBody.gravityScale = 4f; // Turn on gravity when not climbing
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true; // Set the ladder flag to true when colliding with a ladder
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false; // Set the ladder flag to false when leaving a ladder
            isClimbing = false; // Reset climbing flag
            animator.SetBool("IsClimbing", false); // Reset the "IsClimbing" parameter in the animator
            animator.speed = 1; // Restore animator speed to 1
        }
    }
}
