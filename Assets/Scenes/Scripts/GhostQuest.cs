using UnityEngine;

public class GhostQuest : MonoBehaviour
{
    public enum QuestType { Fetch, MorallyDubious, Closure }

    [Header("Quest Type")]
    public QuestType questType;

    [Header("Quest Dialogue")]
    [TextArea] public string questOfferLine;
    [TextArea] public string questAcceptLine;
    [TextArea] public string questDeclineLine;
    [TextArea] public string questIncompleteLine;
    [TextArea] public string questCompleteLine;

    [Header("Quest Morality")]
    public int questMoralityReward;
    public int questDeclineMorality;   //  NEW — morality effect for declining

    [Header("Fetch Quest")]
    public string requiredItemID;

    [Header("Morally Dubious")]
    public string goodItemID;
    public string badItemID;
    public int goodItemMorality;
    public int badItemMorality;

    [Header("Closure Quest")]
    public string requiredNPCID;
    [TextArea] public string closureNPCDialogue;

    private bool playerInRange;
    private bool questAccepted;
    private bool questCompleted;
    private bool awaitingChoice;

    private void Update()
    {
        if (!playerInRange || questCompleted) return;

        // Q = Quest interaction
        if (Input.GetKeyDown(KeyCode.Q) && !questAccepted && !awaitingChoice)
            OfferQuest();

        if (Input.GetKeyDown(KeyCode.Q) && questAccepted)
            TryCompleteQuest();

        // Handle Accept / Decline
        if (awaitingChoice)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) HandleChoice(1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) HandleChoice(2);
        }
    }

    void OfferQuest()
    {
        awaitingChoice = true;

        DialogueUIManager.Instance.Show(
            "Ghost",
            questOfferLine + "\n[1] Accept\n[2] Decline"
        );
    }

    void HandleChoice(int choice)
    {
        if (!awaitingChoice) return;
        awaitingChoice = false;

        if (choice == 1)
        {
            questAccepted = true;

            //  Accept feedback
            DialogueUIManager.Instance.Show("Ghost", questAcceptLine);

            //  UPDATE PROMPT AFTER ACCEPTING
            WorldPromptUI.Instance.Show("Press Q to Return Quest | Press E to Talk", transform);
        }
        else
        {
            //  APPLY DECLINE MORALITY
            MoralityManager.Instance.ModifyMorality(questDeclineMorality);

            DialogueUIManager.Instance.Show("Ghost", questDeclineLine);
        }
    }

    void TryCompleteQuest()
    {
        switch (questType)
        {
            case QuestType.Fetch:
                if (Inventory.Instance.HasItem(requiredItemID))
                {
                    Inventory.Instance.RemoveItem(requiredItemID);
                    CompleteQuest(questMoralityReward);
                }
                else
                {
                    DialogueUIManager.Instance.Show("Ghost", questIncompleteLine);
                }
                break;

            case QuestType.MorallyDubious:
                if (Inventory.Instance.HasItem(goodItemID))
                {
                    Inventory.Instance.RemoveItem(goodItemID);
                    CompleteQuest(goodItemMorality);
                }
                else if (Inventory.Instance.HasItem(badItemID))
                {
                    Inventory.Instance.RemoveItem(badItemID);
                    CompleteQuest(badItemMorality);
                }
                else
                {
                    DialogueUIManager.Instance.Show("Ghost", questIncompleteLine);
                }
                break;

            case QuestType.Closure:
                if (QuestManager.Instance.HasSpokenTo(requiredNPCID))
                {
                    CompleteQuest(questMoralityReward);
                }
                else
                {
                    DialogueUIManager.Instance.Show("Ghost", questIncompleteLine);
                }
                break;
        }
    }

    void CompleteQuest(int morality)
    {
        questCompleted = true;
        MoralityManager.Instance.ModifyMorality(morality);
        DialogueUIManager.Instance.Show("Ghost", questCompleteLine);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        //  Dynamic prompt based on quest state
        if (!questAccepted)
            WorldPromptUI.Instance.Show("Press Q for Quest | Press E to Talk", transform);
        else
            WorldPromptUI.Instance.Show("Press Q to Return Quest | Press E to Talk", transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        awaitingChoice = false;

        //  Clears dialogue when walking away
        DialogueUIManager.Instance.Hide();
        WorldPromptUI.Instance.Hide();
    }
}