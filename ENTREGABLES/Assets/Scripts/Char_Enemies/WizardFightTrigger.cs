using UnityEngine;

public class WizardFightTrigger : MonoBehaviour
{
    [SerializeField] private WizardBoss boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.StartFight();
            gameObject.SetActive(false); // Disable so it doesn't trigger again
        }
    }
}