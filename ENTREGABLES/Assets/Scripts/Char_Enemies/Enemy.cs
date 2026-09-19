using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHittable
{
    protected enum EnemyState {Telegraph, Attack, Vulnerable, Recovery } // Define the possible states for the enemy

    protected int maxHealth = 25;
    protected int currentHealth;
    protected bool isDead = false;
    protected EnemyState currentState; // Define the current state of the enemy
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;
    protected Color currentBaseTint;

    [SerializeField] protected GameObject relicFragmentPrefab;
    
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        currentBaseTint = originalColor; 
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void RegisterHit()
    {
        if (isDead) return;

        currentHealth--;
        

        if (currentHealth <= 0)
        {
            Die();
        }
    }
   
    protected IEnumerator Telegraph(Color color, float duration)
    {
        spriteRenderer.color = color;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = currentBaseTint;
    }

    protected void SetBaseTint(Color color)
    {
        currentBaseTint = color;
        spriteRenderer.color = color;
    }
    protected virtual void Die()
    {
        isDead = true;

        if (relicFragmentPrefab != null)
        {
            Instantiate(relicFragmentPrefab, transform.position, Quaternion.identity);
        }
    }
}

   

