using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private HashSet<string> spokenNPCs = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterSpokenNPC(string npcID)
    {
        if (!spokenNPCs.Contains(npcID))
            spokenNPCs.Add(npcID);
    }

    public bool HasSpokenTo(string npcID)
    {
        return spokenNPCs.Contains(npcID);
    }

    public void ModifyMorality(int amount)
    {
        Debug.Log("Morality changed by: " + amount);
    }
}