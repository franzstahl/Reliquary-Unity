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

    private int maxHP = 15;
    private int currentHP;
    private AudioSource audioSource;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHP -= amount;
        audioSource.PlayOneShot(hitSound);

        if (currentHP <= 0)
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