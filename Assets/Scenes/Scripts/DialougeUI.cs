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
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false);
        IsDialogueActive = false;
        InputBlocked = false;
    }

    // -----------------------------
    // SINGLE CLEAN SHOW FUNCTION
    // -----------------------------
    public void Show(string speakerName, string text, bool blockInput = true)
    {
        dialoguePanel.SetActive(true);
        IsDialogueActive = true;

        if (speakerNameText) speakerNameText.text = speakerName;
        if (dialogueText) dialogueText.text = text;

        if (blockInput)
            InputBlocked = true;
    }

    // -----------------------------
    // SINGLE CLEAN HIDE FUNCTION
    // -----------------------------
    public void Hide()
    {
        dialoguePanel.SetActive(false);

        IsDialogueActive = false;
        InputBlocked = false;
    }
}