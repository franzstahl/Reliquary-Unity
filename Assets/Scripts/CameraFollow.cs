using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
   
    
    private void LateUpdate()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z); // Update camera's position to follow player while maintaining its original z position
    }
}
