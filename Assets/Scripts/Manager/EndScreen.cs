using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu"; 

    private bool hasTransitioned = false; // Prevents spamming LoadScene on multiple key presses in one frame

    private void Update()
    {
        if (!hasTransitioned && Input.anyKeyDown)
        {
            hasTransitioned = true;
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}