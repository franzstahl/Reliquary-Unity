using UnityEngine;

public abstract class Health : MonoBehaviour
{
    protected const int maxHealth = 15;
    protected int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public abstract void TakeDamage(int amount);
}
