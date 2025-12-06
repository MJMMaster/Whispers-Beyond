using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "GhostQuest/Quest")]
public class QuestSO : ScriptableObject
{
    public string questName;
    [TextArea] public string description;
    public QuestType questType;

    // Base morality impact
    public int moralityImpact;

    public enum QuestType
    {
        Fetch,
        Closure,
        MorallyDubious
    }

    [Header("Quest Requirements")]
    public string requiredItemID;   // For fetch quests
    public string requiredNPCID;    // For closure quests

    [Header("Dialogue Text")]
    [TextArea] public string questStartText;
    [TextArea] public string questIncompleteText;
    [TextArea] public string questCompleteText;

    [Header("Morally Dubious Paths")]
    public string humanHeartID;
    public string animalHeartID;

    public int moralityKillPerson = -10;
    public int moralityKillAnimal = -5;
    public int moralityRefuse = 2;
}