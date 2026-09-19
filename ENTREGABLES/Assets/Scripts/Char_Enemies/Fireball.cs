using UnityEngine;

public enum FireballType { Normal, InstaKill, WizardBolt } 
public class Fireball : MonoBehaviour, IHittable
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 4f;

    [Header("Visual")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject explosionPrefab;

    private Rigidbody2D rb;
    private Vector2 direction;
    private FireballType type;
    private bool isReflected;
    private float lifetimeTimer;

    [SerializeField] private AudioClip fireballSound;

    private AudioSource audioSource;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Launch(Vector2 dir, FireballType fireballType)
    {
        direction = dir;
        type = fireballType;
        isReflected = false;
        lifetimeTimer = 0f;
        spriteRenderer.flipX = direction.x < 0;

        audioSource.PlayOneShot(fireballSound);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        lifetimeTimer += Time.fixedDeltaTime;
        if (lifetimeTimer >= lifeTime)
        {
            ReturnToPool();
        }
    }

    public void RegisterHit()
    {
        if (type == FireballType.InstaKill || type == FireballType.WizardBolt) return;

        direction = -direction;
        isReflected = true;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isReflected && other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                if (type == FireballType.InstaKill)
                    health.Die();
                else health.TakeDamage(1);
            }

            ReturnToPool();
        }
        else if (isReflected && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.RegisterHit();
            }

            ReturnToPool();
        }
        else // Hits the barrel
        {
            BarrelStack barrel = other.GetComponent<BarrelStack>();
            if (barrel != null)
            {
                barrel.RegisterHit();
                ReturnToPool();
            }
        }
    }
    private void ReturnToPool()
    {
        SpawnExplosion();
        FireballPool.Instance.ReturnFireball(this);
    }

    private void SpawnExplosion()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
