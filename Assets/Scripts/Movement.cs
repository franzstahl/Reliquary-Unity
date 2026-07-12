using UnityEngine;

public class Movement : MonoBehaviour
{
   [SerializeField] private float speed;
   [SerializeField] private Transform groundCheck; 
   [SerializeField] private float groundCheckRadius;
   [SerializeField] private LayerMask groundLayer; // LayerMask to specify which layers are considered ground
   [SerializeField] private float jumpForce; // Force applied to the player when jumping
   private int maxJumps = 2;
   private int jumpsRemaining; 

   private Rigidbody2D rb;
   private Animator anim;
   private SpriteRenderer sr;


    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    

    void Update()
    {
        // Run logic
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input from player
        anim.SetFloat("Speed", Mathf.Abs(horizontalInput)); // Set "Speed" parameter in Animator based on absolute value of horizontal input
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);  // Set linear velocity of Rigidbody based on horizontal input and speed

        if (horizontalInput < 0)
        {
            sr.flipX = true; // Flip the sprite horizontally when moving left
        }
        else if (horizontalInput > 0)
        {
            sr.flipX = false; // Reset the sprite flip when moving right
        }

        // Jump logic
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // Check if the player is grounded using OverlapCircle
        anim.SetBool("isGrounded", isGrounded); // Set the "isGrounded" parameter in Animator based on whether the player is grounded

        if (isGrounded) // Reset jumps remaining when grounded
        {
            jumpsRemaining = maxJumps;
        }

        if (jumpsRemaining > 0 && Input.GetKeyDown(KeyCode.W)) // Check if player has jumps remaining and if (W) is pressed
        {
            rb.linearVelocity = new Vector2(horizontalInput * speed, jumpForce);
            anim.SetBool("isGrounded", false);
            jumpsRemaining--;
        }

        // Fall animation
        anim.SetFloat("VerticalVelocity", rb.linearVelocity.y); 
     
    }
}
