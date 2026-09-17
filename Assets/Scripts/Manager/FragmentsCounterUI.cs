using UnityEngine;
using TMPro;

public class FragmentsCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fragmentsUI;

    private void Start()
    {
        GameManager.Instance.OnFragmentsChanged += SetFragments;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnFragmentsChanged -= SetFragments;
    }

    public void SetFragments(int current, int total)
    {
        fragmentsUI.text = current + "/" + total;
    }
}