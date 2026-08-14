using UnityEngine;
using System.Collections;

public class BarrelStack : MonoBehaviour
{
    [SerializeField] private int hitsToBreak = 3;
    [SerializeField] private GameObject[] barrelVisuals;
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

        if(hitsTaken >= hitsToBreak)
        {
            barrelCollider.enabled = false;
            StartCoroutine(FadeAllBarrels());
        }

    }

    private IEnumerator FadeAllBarrels()
    {
        SpriteRenderer[] renderers = new SpriteRenderer[barrelVisuals.Length];
        for (int i = 0; i < barrelVisuals.Length; i++)
        {
            renderers[i] = barrelVisuals[i].GetComponent<SpriteRenderer>();
        }

        float elapsed = 0f;

        while(elapsed < hitsTaken)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            foreach (SpriteRenderer sr in renderers)
            {
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }
            yield return null;
        }
        foreach (GameObject barrel in barrelVisuals)
        {
            barrel.SetActive(false);
        }
       
    }
}
