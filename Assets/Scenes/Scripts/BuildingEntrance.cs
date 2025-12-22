using UnityEngine;

public class BuildingEntrance : MonoBehaviour
{
    [Header("Building ID")]
    [Tooltip("Must be UNIQUE per building")]
    public string buildingID;

    [Header("Interior Scene")]
    public string interiorSceneName;

    [Header("Entry Settings")]
    public bool startsLocked = true;
    public string requiredItemID;

    [Header("Morality Effects")]
    public int keyEntryMorality = 0;
    public int breakInMorality = -10;

    private bool playerInRange;

    private void Start()
    {
        // Check persistent unlock state
        if (!startsLocked || BuildingUnlockManager.IsUnlocked(buildingID))
        {
            startsLocked = false;
        }
    }

    private void Update()
    {
        if (!playerInRange) return;

        // Use Key
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryEnterWithKey();
        }

        // Break In
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BreakIn();
        }
    }

    void TryEnterWithKey()
    {
        if (!startsLocked)
        {
            EnterBuilding();
            return;
        }

        if (Inventory.Instance.HasItem(requiredItemID))
        {
            Inventory.Instance.RemoveItem(requiredItemID);
            MoralityManager.Instance.ModifyMorality(keyEntryMorality);

            UnlockBuilding();
            EnterBuilding();
        }
        else
        {
            DialogueUIManager.Instance.Show(
                "System",
                "The door is locked."
            );
        }
    }

    void BreakIn()
    {
        if (!startsLocked) return;

        MoralityManager.Instance.ModifyMorality(breakInMorality);
        UnlockBuilding();
        EnterBuilding();
    }

    void UnlockBuilding()
    {
        startsLocked = false;
        BuildingUnlockManager.Unlock(buildingID);
    }

    void EnterBuilding()
    {
        SceneTransitionManager.Instance.LoadScene(
            interiorSceneName,
            "InteriorSpawn"
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (startsLocked)
        {
            WorldPromptUI.Instance.Show(
                "E: Use Key | Q: Break In",
                transform
            );
        }
        else
        {
            WorldPromptUI.Instance.Show(
                "E: Enter Building",
                transform
            );
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        WorldPromptUI.Instance.Hide();
    }
}