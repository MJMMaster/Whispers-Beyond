using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName = "NPC";
    public string npcID = "npc_1"; // unique ID per NPC

    private bool questCompleted = false;
    private bool playerInRange = false;

    // Called when the player interacts (presses E)
    public void Interact(QuestSO quest)
    {
        if (!playerInRange) return;

        // Already completed
        if (questCompleted)
        {
            DialogueUIManager.Instance.Show(npcName, "We've already spoken about this.");
            return;
        }

        // Closure quest check
        if (quest != null &&
            quest.questType == QuestSO.QuestType.Closure &&
            quest.requiredNPCID == npcID)
        {
            bool completed = QuestManager.Instance.CompleteQuest(quest);

            if (completed)
            {
                questCompleted = true;
                DialogueUIManager.Instance.Show(npcName, "Thank you, you've helped me find peace.");
            }
            else
            {
                DialogueUIManager.Instance.Show(npcName, "You still haven't done what I asked.");
            }
        }
        else
        {
            DialogueUIManager.Instance.Show(npcName, "Hello there.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        WorldPromptUI.Instance.Show("Press E to talk", transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        WorldPromptUI.Instance.Hide();
    }
}