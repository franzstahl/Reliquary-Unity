using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour, IInteractable
{
    [SerializeField] private string nextSceneName; // The name of the next scene to load
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Interact();
        }
        
    }

    public void Interact()
    {
        GameManager.Instance.LoadSceneWithFade(nextSceneName);
    }
}
    

