using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
  

    [SerializeField] private float maxX; // Maximum X position for the camera
    [SerializeField] private float minX; // Minimum X position for the camera

    private float currentMaxY; 
    private float currentMinY; 
    [SerializeField] private float verticalFollowSpeed = 5f;

    private bool followVertical = false;
    private float defaultY; // Store the default Y position of the camera

    private Camera cam;

    [Header("Shake effect")]
    [SerializeField] private float shakeDuration = 0.25f;
    [SerializeField] private float shakeIntensity = 0.3f;
    private Vector3 shakeOffset = Vector3.zero; // Offset to apply to the camera's position during shake
    public static CameraFollow Instance { get; private set; } // Singleton instance for easy access 

    public void SetVerticalFollow (bool enable, float zoneMinY = 0f, float zoneMaxY = 0f) // if the player is in a trigger, enable vertical follow, otherwise disable it
    {
        followVertical = enable;
        currentMinY = zoneMinY;
        currentMaxY = zoneMaxY;
    }
    private void Awake()
    {
        // Singleton pattern to ensure only one instance of CameraFollow exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this; 

        cam = GetComponent<Camera>();
        defaultY = transform.position.y;
    }

    public void TriggerShake()
    {
        Debug.Log("Shake triggered");
        StartCoroutine(ShakeEffect());
    }

    private IEnumerator ShakeEffect()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            shakeOffset = Random.insideUnitCircle * shakeIntensity;
            yield return null;
        }

        shakeOffset = Vector3.zero; // Reset shake offset after shaking
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

        transform.position = new Vector3(clampedX, targetY , transform.position.z) + shakeOffset; // Update the camera's position to follow the player while applying shake offset
    }
}
