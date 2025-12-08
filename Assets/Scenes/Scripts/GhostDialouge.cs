using UnityEngine;

public class GhostDialogue : MonoBehaviour
{
    public string ghostName;

    [TextArea] public string openingLine;

    public string option1Text;
    [TextArea] public string option1Response;
    public int option1Morality;

    public string option2Text;
    [TextArea] public string option2Response;
    public int option2Morality;

    private bool playerInRange;
    private bool awaitingChoice;

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E) && !awaitingChoice)
            ShowDialogue();

        if (awaitingChoice)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ChooseOption(1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ChooseOption(2);
        }
    }

    void ShowDialogue()
    {
        awaitingChoice = true;
        DialogueUIManager.Instance.Show(
            ghostName,
            openingLine +
            "\n[1] " + option1Text +
            "\n[2] " + option2Text
        );
    }

    void ChooseOption(int option)
    {
        awaitingChoice = false;

        if (option == 1)
        {
            MoralityManager.Instance.ModifyMorality(option1Morality);
            DialogueUIManager.Instance.Show(ghostName, option1Response);
        }
        else
        {
            MoralityManager.Instance.ModifyMorality(option2Morality);
            DialogueUIManager.Instance.Show(ghostName, option2Response);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        WorldPromptUI.Instance.Show("Press E to Talk | Press Q for Quest", transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        awaitingChoice = false;
        DialogueUIManager.Instance.Hide();
        WorldPromptUI.Instance.Hide();
    }
}