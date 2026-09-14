using UnityEngine;

public class FightKnightZone : MonoBehaviour
{
    [SerializeField] private KnightBoss knightBoss;

    [SerializeField] private GameObject enterLimit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            knightBoss.StartFight();
            enterLimit.SetActive(true); // Activate the enter limit to prevent the player from leaving the fight area
            gameObject.SetActive(false); // Disable the trigger zone after the fight starts to prevent re-triggering
        }
    }

}
