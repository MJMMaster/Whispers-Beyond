using UnityEngine;

public class MorallyDubiousQuestHandler : MonoBehaviour
{
    public QuestSO quest;

    private bool acceptedQuest = false;
    private bool questResolved = false;

    public void InitializeQuest(QuestSO newQuest)
    {
        quest = newQuest;
        acceptedQuest = false;
        questResolved = false;

        // Show initial choice dialogue
        DialogueUIManager.Instance.Show("Ghost", "Will you help me?\n[1] Accept\n[2] Refuse");
    }

    private void Update()
    {
        if (quest == null || questResolved) return;

        // Player chooses ACCEPT or DECLINE
        if (!acceptedQuest)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                acceptedQuest = true;
                DialogueUIManager.Instance.Show("Ghost", "You accepted.\nBring me a human or animal heart.");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                QuestManager.Instance.ModifyMorality(quest.moralityRefuse);
                DialogueUIManager.Instance.Show("Ghost", "You refused. The ghost fades away...");
                questResolved = true;
            }
        }
    }

    // Called when interacting with ghost again
    public bool TryCompleteHeartTurnIn()
    {
        if (!acceptedQuest || questResolved) return false;

        // Check for human heart
        if (Inventory.Instance.HasItem(quest.humanHeartID))
        {
            Inventory.Instance.RemoveItem(quest.humanHeartID);
            QuestManager.Instance.ModifyMorality(quest.moralityKillPerson);
            DialogueUIManager.Instance.Show("Ghost", "This human heart will do... dark, but necessary.");
            questResolved = true;
            return true;
        }

        // Check for animal heart
        if (Inventory.Instance.HasItem(quest.animalHeartID))
        {
            Inventory.Instance.RemoveItem(quest.animalHeartID);
            QuestManager.Instance.ModifyMorality(quest.moralityKillAnimal);
            DialogueUIManager.Instance.Show("Ghost", "An animal heart... not ideal, but acceptable.");
            questResolved = true;
            return true;
        }

        DialogueUIManager.Instance.Show("Ghost", "You haven't brought me any heart.");
        return false;
    }
}