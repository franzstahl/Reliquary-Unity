using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hitSound;
    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private Rigidbody2D rb;
    private bool isDead = false;

    private const int maxHealth = 15;
    private int currentHealth;
    private AudioSource audioSource;
    

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth; // Set current health to max health at the start
        GameManager.Instance.healthBarUI.SetHealth(currentHealth, maxHealth); // Initialize health bar UI
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0); // Ensure health doesn't go below 0 
        audioSource.PlayOneShot(hitSound);
        GameManager.Instance.healthBarUI.SetHealth(currentHealth, maxHealth); // Update health bar UI each time damage is taken
        CameraFollow.Instance.TriggerShake(); // Trigger camera shake effect when taking damage

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("GetHit");
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        audioSource.PlayOneShot(deathSound);

        playerMovement.SetInputLocked(true);
        playerMovement.ForceGroundedAnimState(); 
        playerAttack.SetInputLocked(true);

        rb.linearVelocity = Vector2.zero; // Stop the player's movement immediately

        anim.SetTrigger("isDead");
    }

    public void DeathAnimationComplete()
    {
        GameManager.Instance.LoseLife();
    }
}