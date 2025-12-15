using UnityEngine;

public class GhostDialogue : MonoBehaviour
{
    public string ghostName;

    [Header("Morality Thresholds")]
    public int goodThreshold = 20;
    public int evilThreshold = -20;

    [Header("GOOD Dialogue")]
    [TextArea] public string goodOpening;
    public string goodOption1Text;
    [TextArea] public string goodOption1Response;
    public int goodOption1Morality;

    public string goodOption2Text;
    [TextArea] public string goodOption2Response;
    public int goodOption2Morality;

    [Header("NEUTRAL Dialogue")]
    [TextArea] public string neutralOpening;
    public string neutralOption1Text;
    [TextArea] public string neutralOption1Response;
    public int neutralOption1Morality;

    public string neutralOption2Text;
    [TextArea] public string neutralOption2Response;
    public int neutralOption2Morality;

    [Header("EVIL Dialogue")]
    [TextArea] public string evilOpening;
    public string evilOption1Text;
    [TextArea] public string evilOption1Response;
    public int evilOption1Morality;

    public string evilOption2Text;
    [TextArea] public string evilOption2Response;
    public int evilOption2Morality;

    private bool playerInRange;
    private bool awaitingChoice;

    // Selected set for this interaction
    private string currentOpening;
    private string currentOpt1;
    private string currentOpt1Response;
    private int currentOpt1Morality;

    private string currentOpt2;
    private string currentOpt2Response;
    private int currentOpt2Morality;

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E) && !awaitingChoice)
            PrepareDialogue();

        if (awaitingChoice)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ChooseOption(1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ChooseOption(2);
        }
    }

    void PrepareDialogue()
    {
        awaitingChoice = true;

        int morality = MoralityManager.Instance.currentMorality;

        // Select dialogue based on morality
        if (morality >= goodThreshold)
            LoadGoodDialogue();
        else if (morality <= evilThreshold)
            LoadEvilDialogue();
        else
            LoadNeutralDialogue();

        // Show dialogue
        DialogueUIManager.Instance.Show(
            ghostName,
            currentOpening +
            "\n[1] " + currentOpt1 +
            "\n[2] " + currentOpt2
        );
    }

    void LoadGoodDialogue()
    {
        currentOpening = goodOpening;
        currentOpt1 = goodOption1Text;
        currentOpt1Response = goodOption1Response;
        currentOpt1Morality = goodOption1Morality;

        currentOpt2 = goodOption2Text;
        currentOpt2Response = goodOption2Response;
        currentOpt2Morality = goodOption2Morality;
    }

    void LoadNeutralDialogue()
    {
        currentOpening = neutralOpening;
        currentOpt1 = neutralOption1Text;
        currentOpt1Response = neutralOption1Response;
        currentOpt1Morality = neutralOption1Morality;

        currentOpt2 = neutralOption2Text;
        currentOpt2Response = neutralOption2Response;
        currentOpt2Morality = neutralOption2Morality;
    }

    void LoadEvilDialogue()
    {
        currentOpening = evilOpening;
        currentOpt1 = evilOption1Text;
        currentOpt1Response = evilOption1Response;
        currentOpt1Morality = evilOption1Morality;

        currentOpt2 = evilOption2Text;
        currentOpt2Response = evilOption2Response;
        currentOpt2Morality = evilOption2Morality;
    }

    void ChooseOption(int option)
    {
        awaitingChoice = false;

        if (option == 1)
        {
            MoralityManager.Instance.ModifyMorality(currentOpt1Morality);
            DialogueUIManager.Instance.Show(ghostName, currentOpt1Response);
        }
        else
        {
            MoralityManager.Instance.ModifyMorality(currentOpt2Morality);
            DialogueUIManager.Instance.Show(ghostName, currentOpt2Response);
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