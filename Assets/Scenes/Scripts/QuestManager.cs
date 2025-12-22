using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    // QuestID  State
    private Dictionary<string, QuestState> questStates =
        new Dictionary<string, QuestState>();

    // Closure NPC tracking
    private HashSet<string> spokenNPCs = new HashSet<string>();

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
        }
    }

    // -----------------------------
    // QUEST STATE MANAGEMENT
    // -----------------------------
    public QuestState GetQuestState(string questID)
    {
        if (!questStates.ContainsKey(questID))
            return QuestState.NotStarted;

        return questStates[questID];
    }

    public void AcceptQuest(string questID)
    {
        questStates[questID] = QuestState.Accepted;
    }

    public void CompleteQuest(string questID)
    {
        questStates[questID] = QuestState.Completed;
    }

    public bool IsQuestAccepted(string questID)
    {
        return GetQuestState(questID) == QuestState.Accepted;
    }

    public bool IsQuestCompleted(string questID)
    {
        return GetQuestState(questID) == QuestState.Completed;
    }

    // -----------------------------
    // CLOSURE NPC TRACKING
    // -----------------------------
    public void RegisterSpokenNPC(string npcID)
    {
        spokenNPCs.Add(npcID);
    }

    public bool HasSpokenTo(string npcID)
    {
        return spokenNPCs.Contains(npcID);
    }
}

public enum QuestState
{
    NotStarted,
    Accepted,
    Completed
}