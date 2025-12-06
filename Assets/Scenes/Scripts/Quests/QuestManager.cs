using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public int playerMorality;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartQuest(QuestSO quest)
    {
        Debug.Log($"Quest Started: {quest.questName}");
    }

    public void ModifyMorality(int amount)
    {
        playerMorality += amount;
        Debug.Log("Player Morality: " + playerMorality);
    }

    public bool CompleteQuest(QuestSO quest)
    {
        if (quest.questType == QuestSO.QuestType.Fetch)
        {
            if (!Inventory.Instance.HasItem(quest.requiredItemID))
                return false;

            Inventory.Instance.RemoveItem(quest.requiredItemID);
        }

        playerMorality += quest.moralityImpact;
        Debug.Log($"Quest Completed: {quest.questName}");
        return true;
    }
}