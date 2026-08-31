using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WormBoss : Enemy
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private List<Transform> firePoints;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Timings")]
    [SerializeField] private float telegraphDuration = 0.8f; // Normal telegraph duration
    [SerializeField] private float recoveryDuration = 1f;
    [SerializeField] private float meleeTelegraphDuration = 0.6f; // Instakill telegraph duration
    private float hitFlashDuration = 0.15f;

    private Color originalColor;
    private Coroutine attackLoopCoroutine;
    private FireballType pendingType;
    private AudioSource audioSource;
    
    
    protected override void Start()
    {
        base.Start();
        originalColor = spriteRenderer.color;
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPlayerEnterDetection()
    {
        if (isDead) return;
        Debug.Log("ENTER detection");
        if (attackLoopCoroutine == null)
        {
            attackLoopCoroutine = StartCoroutine(AttackLoop());
        }
    }

    public void OnPlayerExitDetection()
    {
        Debug.Log("EXIT detection");
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

    public void TriggerInstaKill()
    {
        if (isDead) return;
        StartCoroutine(InstaKill());
    }

    private IEnumerator InstaKill() // Deadly attack when player is close
    {
        yield return Telegraph(Color.orange, meleeTelegraphDuration);
        FireAt(FireballType.InstaKill);
    }

    private IEnumerator Telegraph(Color color, float duration) // Change color to indicate attack telegraph
    {
        spriteRenderer.color = color;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }

    private void FireAt(FireballType type) // Fire a fireball towards the player
    {
        pendingType = type;
        anim.SetTrigger("Attack");
    }

    public void SpawnFireball()
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
            anim.SetTrigger("GetHit");
            audioSource.PlayOneShot(hitSound);
            StartCoroutine(Telegraph(Color.red, hitFlashDuration));
        }
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
