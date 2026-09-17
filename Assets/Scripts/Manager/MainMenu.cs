using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Zone1";
    [SerializeField] private CanvasGroup fadeCanvasGroup; // Full-screen black panel with a CanvasGroup
    [SerializeField] private float fadeDuration;
    [SerializeField] private AudioSource menuMusic;

    public void StartGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ResetGame();
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        float t = 0f;
        float startVolume = menuMusic != null ? menuMusic.volume : 0f; 
        fadeCanvasGroup.blocksRaycasts = true; // Prevent extra clicks during fade

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / fadeDuration);
            fadeCanvasGroup.alpha = Mathf.Clamp01(t / fadeDuration);

            if (menuMusic != null)
                menuMusic.volume = Mathf.Lerp(startVolume, 0f, progress); // Fade music out in sync

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
}