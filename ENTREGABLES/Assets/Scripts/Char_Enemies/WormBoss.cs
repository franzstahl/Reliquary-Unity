using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WormBoss : Enemy
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private List<Transform> firePoints;
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Timings")]
    [SerializeField] private float telegraphDuration; // Normal telegraph duration
    [SerializeField] private float recoveryDuration;
    [SerializeField] private float meleeTelegraphDuration; // Instakill telegraph duration
    private float hitFlashDuration = 0.2f;
  

    private Coroutine attackLoopCoroutine;
    private FireballType pendingType;
    private AudioSource audioSource;
    
    
    protected override void Start()
    {
        base.Start();
        originalColor = spriteRenderer.color;
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPlayerEnterDetection() // Start attacking when the player enters the detection area
    {
        if (isDead) return;
        if (attackLoopCoroutine == null)
        {
            attackLoopCoroutine = StartCoroutine(AttackLoop());
        }
    }

    public void OnPlayerExitDetection() // Stop attacking when the player leaves the detection area
    {
        if (attackLoopCoroutine != null)
        {
            StopCoroutine(attackLoopCoroutine);
            attackLoopCoroutine = null;
            spriteRenderer.color = originalColor;
        }
    }
    private IEnumerator AttackLoop() // Main attack loop for the boss
    {
        while (!isDead)
        {
            yield return Telegraph(Color.yellow, telegraphDuration);
            FireAt(FireballType.Normal);
            yield return new WaitForSeconds(recoveryDuration);
        }
    }

    public void TriggerInstaKill() // Triggered when the player is very close to the boss
    {
        if (isDead) return;
        if (attackLoopCoroutine != null)
        {
            StopCoroutine(attackLoopCoroutine);
            attackLoopCoroutine = null;
        }
        StartCoroutine(InstaKill());
    }

    private IEnumerator InstaKill() // Deadly attack
    {
        yield return Telegraph(Color.cyan, meleeTelegraphDuration);
        FireAt(FireballType.InstaKill);
    }

    private void FireAt(FireballType type) // Set which type of fireball to spawn and trigger the attack animation
    {
        pendingType = type;
        anim.SetTrigger("Attack");
    }

    public void SpawnFireball() // Spawn a fireball at a random fire point and launch it towards the player
    {
        Transform chosenPoint = firePoints[Random.Range(0, firePoints.Count)];
        Fireball fireball = FireballPool.Instance.GetFireball();
        fireball.transform.position = chosenPoint.position;

        Vector2 direction = (player.position.x >= transform.position.x) ? Vector2.right : Vector2.left;
        fireball.Launch(direction, pendingType);
    }

    public override void RegisterHit() // Handles getting hit by the player
    {
        base.RegisterHit();
        if (!isDead)
        {
            StartCoroutine(HitReaction());
        }
    }

    private IEnumerator HitReaction() // Handles the visual and audio feedback when the boss is hit
    {
        if (attackLoopCoroutine != null)
        {
            StopCoroutine(attackLoopCoroutine);
        }

        audioSource.PlayOneShot(hitSound);
        yield return StartCoroutine(Telegraph(Color.red, hitFlashDuration));

        attackLoopCoroutine = StartCoroutine(AttackLoop());

    }

    protected override void Die()
    {
        base.Die();
        StopAllCoroutines();
        anim.SetTrigger("Death");
        bodyCollider.enabled = false;
        audioSource.PlayOneShot(deathSound);
    }
}
