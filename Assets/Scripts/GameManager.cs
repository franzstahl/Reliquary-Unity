using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Everyone can access the GameManager through this property but only the GameManager class can set it.

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

        }
        else
        {

        }
    }
}
