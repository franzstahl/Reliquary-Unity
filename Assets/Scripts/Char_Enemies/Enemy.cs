using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHittable
{
    protected enum EnemyState {Telegraph, Attack, Vulnerable, Recovery } // Define the possible states for the enemy

    protected int maxHealth = 20;
    protected int currentHealth;
    protected bool isDead = false;
    protected EnemyState currentState; // Define the current state of the enemy

    [SerializeField] protected GameObject relicFragmentPrefab;
    

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
   
    protected virtual void Die()
    {
        isDead = true;

        if (relicFragmentPrefab != null)
        {
            Instantiate(relicFragmentPrefab, transform.position, Quaternion.identity);
        }
    }
}

   

