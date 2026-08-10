using UnityEngine;

public class CameraVerticalZone : MonoBehaviour
{

    [SerializeField] private CameraFollow cameraFollow;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraFollow.SetVerticalFollow(true);
        }
    }

  
   //private void OnTriggerExit2D(Collider2D other)
   // {
   //     if (other.CompareTag("Player"))
   //     {
   //         cameraFollow.SetVerticalFollow(false);
   //     }
   // }
}
