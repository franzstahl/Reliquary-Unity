using UnityEngine;
using System.Collections;
public class WizardBoss : Enemy, IHittable
{
    private enum Phase { Attacking, Casting }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject exitLimit;

    [Header("Timings")]
    [SerializeField] private float attackPhaseDuration = 9f;
    [SerializeField] private float castDuration = 5f;
    [SerializeField] private float shotCooldown = 0.7f;
    private float hitFlashDuration = 0.2f;

    [Header("Burst Attack")]
    [SerializeField] private int burstProjectileCount = 8;

    private AudioSource audioSource;
    private bool hasFightStarted = false;
    private Phase currentPhase;
    private float lastShotTime;

    protected override void Start()
    {
        base.Start();
        maxHealth = 35;
        audioSource = GetComponent<AudioSource>();
    }

    public void StartFight()
    {
        if (hasFightStarted) return;

        hasFightStarted = true;
        StartCoroutine(PhaseLoop());
    }

    private IEnumerator PhaseLoop()
    {
        while (!isDead)
        {
            currentPhase = Phase.Attacking;
            yield return new WaitForSeconds(attackPhaseDuration);

            if (isDead) yield break;

            currentPhase = Phase.Casting;
            SetBaseTint(Color.cyan);
            yield return new WaitForSeconds(castDuration);

            if (isDead) yield break;

            SetBaseTint(originalColor);
            FireBurst();
        }
    }

    private void Update()
    {
        if (!hasFightStarted || isDead || currentPhase != Phase.Attacking) return;

        if (Time.time >= lastShotTime + shotCooldown)
        {
            lastShotTime = Time.time;
            anim.SetTrigger("Attack"); // Animation Event calls SpawnFireball at the right frame
        }
    }

    public void SpawnFireball() // Called via Animation Event, fires a single magicball aimed at the player
    {
        Debug.Log("Disparo");
        Vector2 direction = ((Vector2)player.position - (Vector2)firePoint.position).normalized;

        spriteRenderer.flipX = player.position.x < transform.position.x; // Face the player´s side

        Fireball fireball = FireballPool.Instance.GetFireball();
        fireball.transform.position = firePoint.position;
        fireball.Launch(direction, FireballType.WizardBolt);
    }

    private void FireBurst() // Fires several magicballs in a circle after the casting window ends
    {
        for (int i = 0; i < burstProjectileCount; i++)
        {
            float angle = i * (360f / burstProjectileCount) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            Fireball fireball = FireballPool.Instance.GetFireball();
            fireball.transform.position = firePoint.position;
            fireball.Launch(direction, FireballType.WizardBolt);
        }
    }

    public override void RegisterHit() // Handles getting hit by the player and triggers hit reaction
    {
        base.RegisterHit();

        if (!isDead)
        {
            StartCoroutine(HitReaction());
        }
    }

    private IEnumerator HitReaction() // Handles the visual and audio feedback when the boss is hit
    {
        audioSource.PlayOneShot(hurtSound);
        yield return StartCoroutine(Telegraph(Color.red, hitFlashDuration));
    }

    protected override void Die()
    {
        base.Die();

        anim.SetTrigger("Death");
        bodyCollider.enabled = false;
        audioSource.PlayOneShot(deathSound);

        if (exitLimit != null)
        {
            exitLimit.SetActive(false); // Disable the exit limit to allow the player to leave the area after defeating the boss
        }
    }
}


