using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    //[SerializeField] private float verticalOffset; // Vertical offset to keep the player in view (4)

    [SerializeField] private float maxX; // Maximum X position for the camera
    [SerializeField] private float minX; // Minimum X position for the camera

    private float currentMaxY; 
    private float currentMinY; 
    [SerializeField] private float verticalFollowSpeed = 5f;

    private bool followVertical = false;
    private float defaultY; // Store the default Y position of the camera

    private Camera cam; 

    public void SetVerticalFollow (bool enable, float zoneMinY = 0f, float zoneMaxY = 0f) // if the player is in a trigger, enable vertical follow, otherwise disable it
    {
        followVertical = enable;
        currentMinY = zoneMinY;
        currentMaxY = zoneMaxY;
    }
    private void Awake()
    {
        cam = GetComponent<Camera>();
        defaultY = transform.position.y;
    }

    private void LateUpdate() 
    {
        if (playerTransform == null) return; // If playerTransform is not assigned, exit the method to avoid errors

        float halfWidth = cam.orthographicSize * cam.aspect; // Calculate half of the camera's width based on its orthographic size and aspect ratio

        float minXAllowed = minX + halfWidth; 
        float maxXAllowed = maxX - halfWidth;

        float clampedX = Mathf.Clamp(playerTransform.position.x, minXAllowed, maxXAllowed); // Clamp the camera's X position to stay within the defined min and max boundaries

        float targetY = transform.position.y; // Default to the current Y position of the camera

        if (followVertical)
        {
            float halfHeight = cam.orthographicSize;
            float minYAllowed = currentMinY + halfHeight;
            float maxYAllowed = currentMaxY - halfHeight;
            float clampedY = Mathf.Clamp(playerTransform.position.y, minYAllowed, maxYAllowed);

            targetY = Mathf.Lerp(transform.position.y, clampedY, verticalFollowSpeed * Time.deltaTime); // Smoothly interpolate camera's Y position towards clamped Y position of player
        }
        else
        {
            targetY = defaultY; // If vertical follow is disabled, reset the camera's Y position to its default value (when getting out the trigger)
        }

        transform.position = new Vector3(clampedX, targetY , transform.position.z); // Update the camera's position to follow player while keeping Y and Z positions unchanged
    }
}
