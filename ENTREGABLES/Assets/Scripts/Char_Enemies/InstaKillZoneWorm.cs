using UnityEngine;

public class InstaKillZoneWorm : MonoBehaviour
{
    [SerializeField] private WormBoss wormBoss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            wormBoss.TriggerInstaKill();
        }
    }
}
