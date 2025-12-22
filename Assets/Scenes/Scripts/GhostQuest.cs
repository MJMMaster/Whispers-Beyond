using UnityEngine;

public class GhostQuest : MonoBehaviour
{
    public enum QuestType { Fetch, MorallyDubious, Closure }

    [Header("Quest Identity")]
    public string questID; // UNIQUE per ghost (e.g. "Church_EvilGhost_Closure")

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
    public int questDeclineMorality;

    [Header("Fetch Quest")]
    public string requiredItemID;

    [Header("Morally Dubious")]
    public string goodItemID;
    public string badItemID;
    public int goodItemMorality;
    public int badItemMorality;

    [Header("Closure Quest")]
    public string requiredNPCID;

    private bool playerInRange;
    private bool awaitingChoice;

    private void Update()
    {
        if (!playerInRange) return;
        if (QuestManager.Instance.IsQuestCompleted(questID)) return;

        bool accepted = QuestManager.Instance.IsQuestAccepted(questID);

        if (Input.GetKeyDown(KeyCode.Q) && !accepted && !awaitingChoice)
            OfferQuest();

        else if (Input.GetKeyDown(KeyCode.Q) && accepted)
            TryCompleteQuest();

        if (awaitingChoice)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) HandleChoice(true);
            if (Input.GetKeyDown(KeyCode.Alpha2)) HandleChoice(false);
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

    void HandleChoice(bool accepted)
    {
        awaitingChoice = false;

        if (accepted)
        {
            QuestManager.Instance.AcceptQuest(questID);
            DialogueUIManager.Instance.Show("Ghost", questAcceptLine);
            WorldPromptUI.Instance.Show("Press Q to Return Quest | Press E to Talk", transform);
        }
        else
        {
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
                    DialogueUIManager.Instance.Show("Ghost", questIncompleteLine);
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
                    DialogueUIManager.Instance.Show("Ghost", questIncompleteLine);
                break;

            case QuestType.Closure:
                if (QuestManager.Instance.HasSpokenTo(requiredNPCID))
                    CompleteQuest(questMoralityReward);
                else
                    DialogueUIManager.Instance.Show("Ghost", questIncompleteLine);
                break;
        }
    }

    void CompleteQuest(int morality)
    {
        QuestManager.Instance.CompleteQuest(questID);
        MoralityManager.Instance.ModifyMorality(morality);
        DialogueUIManager.Instance.Show("Ghost", questCompleteLine);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;

        bool accepted = QuestManager.Instance.IsQuestAccepted(questID);

        WorldPromptUI.Instance.Show(
            accepted ? "Press Q to Return Quest | Press E to Talk"
                     : "Press Q for Quest | Press E to Talk",
            transform
        );
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