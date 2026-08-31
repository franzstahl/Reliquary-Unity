using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHittable
{
    protected enum EnemyState {Telegraph, Attack, Vulnerable, Recovery } // Define the possible states for the enemy

    protected int maxHealth = 15;
    protected int currentHealth;
    protected bool isDead = false;
    protected EnemyState currentState; // Define the current state of the enemy

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
    }
}
