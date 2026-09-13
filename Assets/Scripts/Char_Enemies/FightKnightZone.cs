using UnityEngine;

public class FightKnightZone : MonoBehaviour
{
    [SerializeField] private KnightBoss knightBoss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            knightBoss.StartFight();
            gameObject.SetActive(false); // Disable the trigger zone after the fight starts to prevent re-triggering
        }
    }

}
