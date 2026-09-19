using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private List<Sprite> healthSprites;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnHealthChanged += SetHealth;

            SetHealth(
                GameManager.Instance.CurrentHealth,
                GameManager.Instance.MaxHealth
            );
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnHealthChanged -= SetHealth;
        }
    }

    public void SetHealth(int currentHealth, int maxHealth)
    {
        healthImage.sprite = healthSprites[maxHealth - currentHealth];
    }
}