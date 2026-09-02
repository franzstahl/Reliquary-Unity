using Unity.VisualScripting;
using UnityEngine;

public class RelicFragment : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float bobAmplitude = 0.2f;
    [SerializeField] private float bobSpeed = 2f;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float offsetY = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        transform.position = startPosition + Vector3.up * offsetY;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            GameManager.Instance.CollectFragment();
            Destroy(gameObject);
        }
    }

}
