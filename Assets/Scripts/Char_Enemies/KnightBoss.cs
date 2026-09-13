using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class KnightBoss : Enemy, IHittable
{
    private enum Phase {  Attacking, Vulnerable}

    [SerializeField] private float speed;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float damageRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    [SerializeField] private int attackPhaseDuration;
    [SerializeField] private int vulnerablePhaseDuration;

    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audioSource;
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private float lastAttackTime;
    private Phase currentPhase;

    private bool hasFightStarted = false;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
   private IEnumerator PhaseLoop() // Decides the current phase of the boss and handles the timing for each phase
    {
        while (!isDead)
        {
            currentPhase = Phase.Attacking;
            yield return new WaitForSeconds(attackPhaseDuration);

            currentPhase = Phase.Vulnerable;
            rb.linearVelocity = Vector2.zero; // Stop movement during vulnerable phase
            anim.SetBool("isMoving", false); // Force Idle Animation
            yield return new WaitForSeconds(vulnerablePhaseDuration);
        }
    }

    public void StartFight()
    {
        if (hasFightStarted) return;

        hasFightStarted = true;
        StartCoroutine(PhaseLoop());
    }

    private void FixedUpdate() // Handles boss's movement and attack logic based on the current phase (running towards the player and attacking when in range)
    {
        if (!hasFightStarted ||isDead || currentPhase != Phase.Attacking) return;

        float distanceToPlayer = Mathf.Abs(playerTransform.position.x - rb.position.x); // Calculate the horizontal distance to the player

        if (distanceToPlayer > attackRange)
        {
            float directionX = Mathf.Sign(playerTransform.position.x - rb.position.x);
            Vector2 horizontalMove = new Vector2(directionX, 0); // Calculate the direction to move towards the player
            rb.MovePosition(rb.position + horizontalMove * speed * Time.fixedDeltaTime); // Move the boss towards the player

            spriteRenderer.flipX = directionX < 0; // Flip the sprite based on the direction of movement

            anim.SetBool("isMoving", true);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isMoving", false);
            TryAttack();
        }
    }

    private void TryAttack() // Controls when the boss can attack based on the cooldown and triggers the attack animation
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            anim.SetTrigger("Attack");
            audioSource.PlayOneShot(attackSound);

            StartCoroutine(DamageDelay());
        }
    }

    private IEnumerator DamageDelay() // Applies damage to the player after a short delay to sync with the attack animation
    {
        yield return new WaitForSeconds(0.4f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, damageRange, playerLayer);
        foreach (Collider2D hit in hits)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(2);
            }
        }
    }

    public override void RegisterHit()
    {
        if (isDead) return;

        audioSource.PlayOneShot(hurtSound);
        base.RegisterHit();
    }

    protected override void Die()
    {
        base.Die();

        rb.linearVelocity = Vector2.zero;
        anim.SetTrigger("Death");
        audioSource.PlayOneShot(deathSound);

        GetComponent<Collider2D>().enabled = false;
    }
}


