using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private List<Sprite> healthSprites;

    public void SetHealth(int currentHealth, int maxHealth)
    {
        healthImage.sprite = healthSprites[maxHealth - currentHealth];
    }


}
