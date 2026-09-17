using UnityEngine;
using TMPro;
public class LivesCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesNumber;

    public void SetLives(int amount)
    {
        livesNumber.text = amount.ToString();
    }
    
}
