using UnityEngine;

public class BarrelStack : MonoBehaviour
{
    [SerializeField] private int hitsToBreak = 3;
    [SerializeField] private GameObject[] barrelVisuals;
    [SerializeField] private AudioClip barrelSound;
    private int hitsTaken;
    private Collider2D barrelCollider;

    private AudioSource audioSource;
    private void Start()
    {
        barrelCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    
    public void RegisterHit()
    {
        hitsTaken++;
        audioSource.PlayOneShot(barrelSound);

        if(hitsTaken >= hitsToBreak)
        {
            BreakAllBarrels();
        }

    }

    private void BreakAllBarrels()
    {
        foreach (GameObject barrel in barrelVisuals)
        {
            barrel.SetActive(false);
        }
        barrelCollider.enabled = false;
    }
}
