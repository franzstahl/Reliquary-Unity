using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   [SerializeField] private float speed;
   [SerializeField] private Transform groundCheck; 
   [SerializeField] private float groundCheckRadius;
   [SerializeField] private LayerMask groundLayer; // LayerMask to specify which layers are considered ground
   [SerializeField] private float jumpForce; // Force applied to the player when jumping
   [SerializeField] private float dashForce; 
   [SerializeField] private float dashCooldown = 2.5f;
   [SerializeField] private float dashDuration = 0.33f;

   private float lastDashTime;
   private float dashDirection;
   private bool isDashing;
   

   private int maxJumps = 2;
   private int jumpsRemaining; 


   private Rigidbody2D rb;
   private Animator anim;
   private SpriteRenderer sr;


    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void EndDash() // Method to end the dash after the dash duration
    {
        isDashing = false; // Reset the dashing state to false after the dash duration ends
        anim.SetBool("isDashing", false);
    }
    

    private void Update()
    {
        if (isDashing) return; // If the player is currently dashing, skip the rest of the Update logic

        // Run logic
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input from player
        anim.SetFloat("Speed", Mathf.Abs(horizontalInput)); // Set "Speed" parameter in Animator based on absolute value of horizontal input
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);  // Set linear velocity Rigidbody based on horizontal input and speed

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

        // Dash logic

        if (sr.flipX) // Check if the sprite is flipped (facing left)
        {
            dashDirection = -1;
        }
        else
        {
            dashDirection = 1;
        }

        if (lastDashTime + dashCooldown < Time.time && Input.GetMouseButtonDown(1)) // Check if dash cooldown has passed and if the right mouse button is pressed
        {
            lastDashTime = Time.time; // Update the last dash time to current time
            isDashing = true;
            anim.SetBool("isDashing", true);
            rb.linearVelocity = new Vector2(dashDirection * dashForce, 0);
            anim.SetTrigger("Dash");
            Invoke(nameof(EndDash), dashDuration);
        }
     

    }
}
