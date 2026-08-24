using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   [SerializeField] private float speed; // Horizontal movement

   [SerializeField] private AudioClip dashSound;
   [SerializeField] private AudioClip jumpSound;

   // Jump
   [SerializeField] private Transform groundCheck; 
   [SerializeField] private float groundCheckRadius;
   [SerializeField] private LayerMask groundLayer; // LayerMask to specify which layers are considered ground
   [SerializeField] private float jumpForce;
   [SerializeField] private bool canJump = false;
    

   // Dash
   [SerializeField] private float dashCooldown;
   [SerializeField] private float dashDuration;
   [SerializeField] private float dashDistance;
   [SerializeField] private bool canDash = false;
   private Vector2 dashTarget; // Destination point
   private float lastDashTime;
   private float dashDirection;
   private bool isDashing;
   

   // Double jump
   private int maxJumps = 2;
   private int jumpsRemaining; 


   private Rigidbody2D rb;
   private Animator anim;
   private SpriteRenderer sr;
   private AudioSource audioSource;

   public bool IsGrounded { get; private set; } // Camp property to expose to other scripts the grounded state
   private bool inputLocked;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetInputLocked(bool locked)
    {
        inputLocked = locked; // Set the inputLocked variable to control whether player input is locked or not

        if (locked)
        {
            // Reset animation parameters when input is locked
            anim.SetFloat("Speed", 0);
            anim.SetFloat("VerticalVelocity", 0);
            anim.SetBool("isGrounded", true); 
            anim.SetBool("isDashing", false);
        }
    }
    public void EnableJump()
    {
        canJump = true;
    }

    //--------------------------------------------------------------------------------
    // Dash logic
    private void EndDash() // Method to end the dash after the dash duration
    {
        isDashing = false; // Reset the dashing state to false after the dash duration ends
        anim.SetBool("isDashing", false);
    }

    private void FixedUpdate() 
    {
        if (isDashing)
        {
            float step = (dashDistance / dashDuration) * Time.fixedDeltaTime; // Calculate step size for moving towards the dash target based on dash distance and duration
            Vector2 newPosition = Vector2.MoveTowards(rb.position, dashTarget, step); // Move the player towards the dash target position at a constant speed
            rb.MovePosition(newPosition); // Move the Rigidbody to the new position
        }
    }
    //--------------------------------------------------------------------------------

    private void Update()
    {
        if (inputLocked) return;
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

        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // Check if the player is grounded using OverlapCircle
        anim.SetBool("isGrounded", IsGrounded);

        if (IsGrounded && rb.linearVelocity.y <= 0.1) // Reset jumps remaining when grounded
        {
            jumpsRemaining = maxJumps; 
        }

        if (jumpsRemaining > 0 && canJump && Input.GetKeyDown(KeyCode.W)) // Check if player has jumps remaining and if (W) is pressed
        {
            rb.linearVelocity = new Vector2(horizontalInput * speed, jumpForce); // Apply jump force to the player
            anim.SetBool("isGrounded", false);
            audioSource.PlayOneShot(jumpSound);
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

        if (lastDashTime + dashCooldown < Time.time && canDash && Input.GetMouseButtonDown(1)) // Check if dash cooldown has passed and if the right mouse button is pressed
        {
            lastDashTime = Time.time; // Update the last dash time to current time
            isDashing = true;
            anim.SetBool("isDashing", true);
            dashTarget = rb.position + new Vector2(dashDirection * dashDistance, 0);
            anim.SetTrigger("Dash");
            audioSource.PlayOneShot(dashSound);
            Invoke(nameof(EndDash), dashDuration); // Schedule the EndDash method to be called after dash duration
        }
     

    }
}
