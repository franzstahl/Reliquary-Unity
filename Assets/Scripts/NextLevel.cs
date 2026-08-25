using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // The name of the next scene to load
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Ok");
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.LoadSceneWithFade(nextSceneName);
        }
        
    }

    
}
