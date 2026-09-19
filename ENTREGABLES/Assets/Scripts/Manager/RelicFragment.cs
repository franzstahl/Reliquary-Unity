using Unity.VisualScripting;
using UnityEngine;

public class RelicFragment : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float bobAmplitude;
    [SerializeField] private float bobSpeed;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;

    private int pickupDelay = 2;
    private bool canBePickedUp = false;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        Invoke(nameof(EnablePickup), pickupDelay); // Delay the pickup availability to prevent immediate collection upon spawn.
    }

    private void EnablePickup()
    {
        canBePickedUp = true;
    }

    private void Update() // Responsible for creating a bobbing effect.It calculates a vertical offset using a sine wave based on the current time, bob speed, and amplitude, and then updates the fragment's position accordingly.
    {
        float offsetY = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        transform.position = startPosition + Vector3.up * offsetY;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canBePickedUp) return;
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            GameManager.Instance.CollectFragment();
            Destroy(gameObject);
        }
    }

}
