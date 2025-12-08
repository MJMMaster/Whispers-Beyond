using TMPro;
using UnityEngine;

public class MoralityUI : MonoBehaviour
{
    public TMP_Text moralityText;

    private void Start()
    {
        // Force initial update
        UpdateMorality(MoralityManager.Instance.currentMorality);

        // Subscribe to changes
        MoralityManager.Instance.OnMoralityChanged += UpdateMorality;
    }

    void UpdateMorality(int newValue)
    {
        moralityText.text = "Morality: " + newValue;
    }

    private void OnDestroy()
    {
        MoralityManager.Instance.OnMoralityChanged -= UpdateMorality;
    }
}