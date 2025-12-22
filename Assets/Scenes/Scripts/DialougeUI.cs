using UnityEngine;
using TMPro;

public class DialogueUIManager : MonoBehaviour
{
    public static DialogueUIManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;

    public bool IsDialogueActive { get; private set; }
    public static bool InputBlocked { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        IsDialogueActive = false;
        InputBlocked = false;
    }

    /// <summary>
    /// Show dialogue
    /// </summary>
    public void Show(string speakerName, string text, bool blockInput = true)
    {
        if (dialoguePanel == null || speakerNameText == null || dialogueText == null) return;

        dialoguePanel.SetActive(true);
        IsDialogueActive = true;

        speakerNameText.text = speakerName;
        dialogueText.text = text;

        InputBlocked = blockInput;
    }

    /// <summary>
    /// Hide dialogue
    /// </summary>
    public void Hide()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        IsDialogueActive = false;
        InputBlocked = false;
    }
}