using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private List<Sprite> healthSprites;

    // This method updates the health bar UI based on the player's current and maximum health.
    // Using a list of sprites allows for a more flexible and scalable approach to representing different health states.
    public void SetHealth(int currentHealth, int maxHealth)
    {
        healthImage.sprite = healthSprites[maxHealth - currentHealth];
    }


}
