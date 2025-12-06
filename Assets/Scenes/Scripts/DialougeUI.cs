using UnityEngine;
using TMPro;

public class DialogueUIManager : MonoBehaviour
{
    public static DialogueUIManager Instance;

    public GameObject dialoguePanel;          // Enable/Disable
    public TextMeshProUGUI speakerNameText;   // Text component for who's speaking
    public TextMeshProUGUI dialogueText;      // Text component for the dialogue

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    /// <summary>
    /// Show dialogue with speaker name and text
    /// </summary>
    public void Show(string speakerName, string text)
    {
        dialoguePanel.SetActive(true);

        if (speakerNameText != null)
            speakerNameText.text = speakerName;

        if (dialogueText != null)
            dialogueText.text = text;
        Debug.Log("Showing dialogue: " + speakerName + " - " + text);
    }

    public void Hide()
    {
        dialoguePanel.SetActive(false);
    }
}