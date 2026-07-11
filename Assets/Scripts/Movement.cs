using UnityEngine;

public class Movement : MonoBehaviour
{
   [SerializeField] private float speed;
   [SerializeField] private Transform groundCheck; 
   [SerializeField] private float groundCheckRadius;
   [SerializeField] private LayerMask groundLayer; // LayerMask to specify which layers are considered ground
   [SerializeField] private float jumpForce; // Force applied to the player when jumping
   private Rigidbody2D rb;
   private Animator anim;
   private SpriteRenderer sr;


    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // Check if the player is grounded using OverlapCircle
        anim.SetBool("isGrounded", isGrounded); // Set the "isGrounded" parameter in the Animator based on whether the player is grounded

        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input from player
        anim.SetFloat("Speed", Mathf.Abs(horizontalInput)); // Set the "Speed" parameter in the Animator based on the absolute value of horizontal input
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);  // Set the linear velocity of the Rigidbody2D based on horizontal input and speed

        if (horizontalInput < 0)
        {
            sr.flipX = true; // Flip the sprite horizontally when moving left
        }
        else if (horizontalInput > 0)
        {
            sr.flipX = false; // Reset the sprite flip when moving right
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.W)) // Check if the player is grounded and the jump key (W) is pressed
        {
            rb.linearVelocity = new Vector2(horizontalInput * speed, jumpForce);
            anim.SetBool("isGrounded", false);
        }

        Debug.Log("isGrounded: " + isGrounded + " | Speed: " + Mathf.Abs(horizontalInput));
    }
}
