using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FragmentsCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fragmentsUI;

    public void SetFragments(int current, int total)
    {
        fragmentsUI.text = current + "/" + total;
    }
}
