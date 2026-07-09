using UnityEngine;

public class Movement : MonoBehaviour
{
   [SerializeField] private float speed;
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
        anim.SetBool("isGrounded", true);
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
    }
}
