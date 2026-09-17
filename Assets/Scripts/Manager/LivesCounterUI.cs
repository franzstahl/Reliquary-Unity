using UnityEngine;
using TMPro;

public class LivesCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesNumber;

    private void Start()
    {
        GameManager.Instance.OnLivesChanged += SetLives;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLivesChanged -= SetLives;
    }

    public void SetLives(int amount)
    {
        livesNumber.text = amount.ToString();
    }
}