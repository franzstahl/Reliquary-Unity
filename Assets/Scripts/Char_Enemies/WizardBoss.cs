using UnityEngine;
using System.Collections;
public class WizardBoss : Enemy
{
    private enum Phase { Attacking, Casting }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    //[SerializeField] private AudioClip attackSound;

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

            currentPhase = Phase.Casting;
            SetBaseTint(Color.cyan);
            yield return new WaitForSeconds(castDuration);

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
        Vector2 direction = ((Vector2)player.position - (Vector2)firePoint.position).normalized;

        Fireball fireball = FireballPool.Instance.GetFireball();
        fireball.transform.position = firePoint.position;
        fireball.Launch(direction, FireballType.Normal);
    }

    private void FireBurst() // Fires several magicballs in a circle after the casting window ends
    {
        for (int i = 0; i < burstProjectileCount; i++)
        {
            float angle = i * (360f / burstProjectileCount) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            Fireball fireball = FireballPool.Instance.GetFireball();
            fireball.transform.position = firePoint.position;
            fireball.Launch(direction, FireballType.Normal);
        }
    }

    public override void RegisterHit()
    {
        base.RegisterHit();

        if (!isDead)
        {
            audioSource.PlayOneShot(hurtSound);
        }
    }

    protected override void Die()
    {
        base.Die();

        anim.SetTrigger("Death");
        bodyCollider.enabled = false;
        audioSource.PlayOneShot(deathSound);
    }
}

