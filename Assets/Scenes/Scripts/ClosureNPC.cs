using UnityEngine;

public class ClosureNPC : MonoBehaviour
{
    public string npcID;
    [TextArea] public string closureDialogue;

    private bool playerInRange;
    private bool hasSpoken;

    private void Update()
    {
        if (!playerInRange || hasSpoken) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            hasSpoken = true;

            DialogueUIManager.Instance.Show("NPC", closureDialogue);
            QuestManager.Instance.RegisterSpokenNPC(npcID);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        WorldPromptUI.Instance.Show("Press Q for Quest | Press E to Talk", transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        WorldPromptUI.Instance.Hide();
        DialogueUIManager.Instance.Hide();
    }
}