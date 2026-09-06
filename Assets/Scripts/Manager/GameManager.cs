using System.Collections;
using System.Timers;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Everyone can access the GameManager but only the GameManager class can set it.

    public int FragmentsCollected { get; private set; } = 0; // Tracks the number of relic fragments collected by the player.

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private int totalFragments = 3;
    private float fadeDuration = 2.5f;
    private int maxLives = 3;
    private int currentLives;

    public  HealthbarUI healthBarUI;
    public LivesCounterUI livesCounterUI;
    public FragmentsCounterUI fragmentsUI;
    private void Awake()
    {
        // Prevent multiple instances of GameManager
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;
        }
        Instance = this; 
        DontDestroyOnLoad(gameObject);
        currentLives = maxLives;
    }

    public void CollectFragment()
    {
        FragmentsCollected++;
        fragmentsUI.SetFragments(FragmentsCollected, totalFragments); // Update the fragments counter UI to reflect the current number of collected fragments.
    }
   
    public void LoseLife()
    {
        currentLives--;

        if (currentLives <= 0)
        {
            LoadSceneWithFade("Lose");
        }
        else
        {
            livesCounterUI.SetLives(currentLives); // Update the lives counter UI to reflect the current number of lives.
            LoadSceneWithFade(SceneManager.GetActiveScene().name);
        }
    }

    public void LoadSceneWithFade(string sceneName) // Called to load a new scene with a fade effect.
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName) // Handles the fade effect and scene loading.
    {
        yield return StartCoroutine(Fade(1f)); // Wait for the fade in to complete before loading the new scene.

        SceneManager.LoadScene(sceneName);

        yield return StartCoroutine(Fade(0f)); // Wait for the fade out to complete after loading the new scene.
    }

    private IEnumerator Fade(float targetAlpha) // Handles the fade effect logic by interpolating the alpha value of CanvasGroup over time.
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float elaped = 0f;

        while (elaped < fadeDuration)
        {
            elaped += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elaped / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}
