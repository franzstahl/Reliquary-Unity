using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hitSound;

    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private Rigidbody2D rb;

    private bool isDead = false;

    private AudioSource audioSource;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        base.Start();

        GameManager.Instance.NotifyHealthChanged(
            currentHealth,
            maxHealth
        );
    }

    public override void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);

        audioSource.PlayOneShot(hitSound);

        GameManager.Instance.NotifyHealthChanged(
            currentHealth,
            maxHealth
        );

        CameraFollow.Instance.TriggerShake();

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

        rb.linearVelocity = Vector2.zero;

        anim.SetTrigger("isDead");
    }

    public void DeathAnimationComplete()
    {
        GameManager.Instance.LoseLife();
    }
}