using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    //[SerializeField] private float verticalOffset; // Vertical offset to keep the player in view (4)

    [SerializeField] private float maxX; // Maximum X position for the camera
    [SerializeField] private float minX; // Minimum X position for the camera

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return; // If playerTransform is not assigned, exit the method to avoid errors

        float halfWidth = cam.orthographicSize * cam.aspect; // Calculate half of the camera's width based on its orthographic size and aspect ratio

        float minXAllowed = minX + halfWidth; 
        float maxXAllowed = maxX - halfWidth;

        float clampedX = Mathf.Clamp(playerTransform.position.x, minXAllowed, maxXAllowed); // Clamp the camera's X position to stay within the defined min and max boundaries

        transform.position = new Vector3(clampedX, transform.position.y , transform.position.z); // Update the camera's position to follow player with a vertical offset
    }
}
