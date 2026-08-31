using UnityEngine;

public class CameraVerticalZone : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private float maxY;
    [SerializeField] private float minY;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraFollow.SetVerticalFollow(true,minY, maxY); // Enable vertical follow when the player enters the trigger
        }
    }
}
