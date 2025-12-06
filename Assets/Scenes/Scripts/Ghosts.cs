using UnityEngine;

public class Ghost : MonoBehaviour
{
    public string ghostName = "Unnamed Ghost";
    public QuestSO ghostQuest;

    private bool playerInRange = false;
    private bool questStarted = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        if (!playerInRange) return;

        // FIRST INTERACTION: START QUEST
        if (!questStarted)
        {
            questStarted = true;
            QuestManager.Instance.StartQuest(ghostQuest);

            // Give quest to player
            PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
            if (player != null)
                player.activeQuest = ghostQuest;

            // Initialize morally dubious quest handler if needed
            if (ghostQuest.questType == QuestSO.QuestType.MorallyDubious)
            {
                var handler = GetComponent<MorallyDubiousQuestHandler>();
                if (handler != null)
                    handler.InitializeQuest(ghostQuest);
            }

            // Show dialogue
            DialogueUIManager.Instance.Show(ghostName, ghostQuest.description);
            return;
        }

        // FETCH QUEST RETURN
        if (ghostQuest.questType == QuestSO.QuestType.Fetch)
        {
            if (QuestManager.Instance.CompleteQuest(ghostQuest))
                DialogueUIManager.Instance.Show(ghostName, "Thank you!");
            else
                DialogueUIManager.Instance.Show(ghostName, "You still haven't brought me what I need.");
        }

        // MORALLY DUBIOUS QUEST RETURN
        if (ghostQuest.questType == QuestSO.QuestType.MorallyDubious)
        {
            var handler = GetComponent<MorallyDubiousQuestHandler>();
            if (handler != null && handler.TryCompleteHeartTurnIn())
            {
                DialogueUIManager.Instance.Show(ghostName, "The ritual is complete...");
            }
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