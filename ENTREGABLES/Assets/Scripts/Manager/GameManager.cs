using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int FragmentsCollected { get; private set; } = 0;

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private int totalFragments = 3;
    [SerializeField] private AudioSource mainMusicSource;

    private float fadeDuration = 2.5f;
    private int maxLives = 3;
    private int currentLives;

    private Dictionary<string, bool> triggeredDialogues = new Dictionary<string, bool>();

    public bool hasDialogueTriggered(string id) => triggeredDialogues.ContainsKey(id);

    public void MarkDialogueTriggered(string id) => triggeredDialogues.Add(id, true);

    // Observer pattern
    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnLivesChanged;
    public event Action<int, int> OnFragmentsChanged;

    public int CurrentHealth { get; private set; } = 15;
    public int MaxHealth { get; private set; } = 15;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentLives = maxLives;
    }

    public void NotifyHealthChanged(int currentHealth, int maxHealth)
    {
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void CollectFragment()
    {
        FragmentsCollected++;

        OnFragmentsChanged?.Invoke(
            FragmentsCollected,
            totalFragments
        );
    }
    public void LoseLife()
    {
        currentLives--;

        OnLivesChanged?.Invoke(currentLives);

        if (currentLives <= 0)
        {
            LoadSceneWithFade("Lose");
        }
        else
        {
            LoadSceneWithFade(SceneManager.GetActiveScene().name);
        }
    }

    public void ResetGame()
    {
        currentLives = maxLives;
        FragmentsCollected = 0;
        triggeredDialogues.Clear();

        CurrentHealth = MaxHealth;
        if (!mainMusicSource.isPlaying)
            mainMusicSource.Play();

        OnLivesChanged?.Invoke(currentLives);
        OnFragmentsChanged?.Invoke(
            FragmentsCollected,
            totalFragments
        );
    }
    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        yield return StartCoroutine(Fade(1f));

        SceneManager.LoadScene(sceneName);

        if (sceneName == "Win" || sceneName == "Lose")
        {
            mainMusicSource.Stop();
        }
        else if (!mainMusicSource.isPlaying)
        {
            mainMusicSource.Play();
        }

        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float elaped = 0f;

        while (elaped < fadeDuration)
        {
            elaped += Time.deltaTime;

            fadeCanvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                elaped / fadeDuration
            );

            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
        fadeCanvasGroup.blocksRaycasts = targetAlpha > 0.01f;
    }
}