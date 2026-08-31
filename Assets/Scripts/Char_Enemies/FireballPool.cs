using UnityEngine;
using System.Collections.Generic;
public class FireballPool : MonoBehaviour
{
    public static FireballPool Instance { get; private set; }

    [SerializeField] private Fireball fireballPrefab;
    [SerializeField] private int initialPoolSize = 10;

    private List<Fireball> pool = new List<Fireball>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        for (int i = 0; i< initialPoolSize; i++)
        {
            CreateNewFireball();
        }
    }

    
    private Fireball CreateNewFireball()
    {
        Fireball fireball = Instantiate(fireballPrefab, transform);
        fireball.gameObject.SetActive(false);
        pool.Add(fireball);
        return fireball;
    }

    public Fireball GetFireball()
    {
        foreach (Fireball fireball in pool)
        {
            if (!fireball.gameObject.activeInHierarchy)
            {
                fireball.gameObject.SetActive(true);
                return fireball;
            }
        }

        Fireball newFireball = CreateNewFireball();
        newFireball.gameObject.SetActive(true);
        return newFireball;
    }

    public void ReturnFireball(Fireball fireball)
    {
        fireball.gameObject.SetActive(false);
    }
}
