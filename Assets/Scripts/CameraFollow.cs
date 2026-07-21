using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float verticalOffset; // Vertical offset to keep the player in view


    private void LateUpdate()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + verticalOffset, transform.position.z); // Update the camera's position to follow player with a vertical offset
    }
}
