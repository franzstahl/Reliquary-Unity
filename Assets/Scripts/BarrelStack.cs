using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BarrelStack : MonoBehaviour, IHittable
{
    [SerializeField] private int hitsToBreak = 3;
    [SerializeField] private List<GameObject> barrelVisuals;
    [SerializeField] private AudioClip barrelSound;
    private int hitsTaken = 0;
    private Collider2D barrelCollider;
    private float fadeDuration = 1.5f;
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

        if (hitsTaken >= hitsToBreak)
        {
            barrelCollider.enabled = false;
            StartCoroutine(FadeAllBarrels());
        }

    }

    private IEnumerator FadeAllBarrels()
    {
        List<SpriteRenderer> renderers = new List<SpriteRenderer>(); // Create a list to hold the SpriteRenderer components of all barrel visuals
        foreach (GameObject barrel in barrelVisuals) // Get the SpriteRenderer components of all barrel visuals
        {
            renderers.Add(barrel.GetComponent<SpriteRenderer>()); // Get the SpriteRenderer component of the current barrel visual
        }

        float elapsed = 0f;

        while (elapsed < hitsTaken)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            foreach (SpriteRenderer sr in renderers)
            {
                Color c = sr.color; // Get the current color of the SpriteRenderer
                c.a = alpha; // Set the alpha value of the color to the calculated alpha
                sr.color = c; // Set the color of the SpriteRenderer to the new color with the updated alpha
            }
            yield return null;
        }
        foreach (GameObject barrel in barrelVisuals)
        {
            barrel.SetActive(false);
        }
       
    }
}
