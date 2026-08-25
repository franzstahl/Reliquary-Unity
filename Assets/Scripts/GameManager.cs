using System.Collections;
using System.Timers;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Everyone can access the GameManager through this property but only the GameManager class can set it.

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    private float fadeDuration = 2f;
    private int maxLives = 3;
    private int currentLives;

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

   
    public void LoseLife()
    {
        currentLives--;

        if (currentLives <= 0)
        {
            LoadSceneWithFade("Lose");
        }
        else
        {
            LoadSceneWithFade(SceneManager.GetActiveScene().name);
        }
    }

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        yield return StartCoroutine(Fade(1f));

        SceneManager.LoadScene(sceneName);

        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
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
