using UnityEngine;

public class DetectionZoneWorm : MonoBehaviour
{
    [SerializeField] private WormBoss wormBoss;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            wormBoss.OnPlayerEnterDetection();
        }
     
    }

  
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            wormBoss.OnPlayerExitDetection();
        }
    }
}
