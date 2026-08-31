using UnityEngine;

public class ExitVerticalCamera : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraFollow.SetVerticalFollow(false); // Disable vertical follow when the player enters the trigger
        }
    }

   
   
}
