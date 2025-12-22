using UnityEngine;

public class MoralityShrine : MonoBehaviour
{
    [Header("Settings")]
    public int moralityGainPerUse = 5;
    public int maxUses = 3;

    private int currentUses = 0;
    private bool playerInRange = false;

    private void Update()
    {
        if (!playerInRange) return;

        // Player interacts with E
        if (Input.GetKeyDown(KeyCode.E) && currentUses < maxUses)
        {
            GainMorality();
        }
    }

    private void GainMorality()
    {
        currentUses++;
        MoralityManager.Instance.ModifyMorality(moralityGainPerUse);

        // Optionally show some UI feedback
        DialogueUIManager.Instance.Show(
            "Shrine",
            $"You feel a sense of peace. ({currentUses}/{maxUses})"
        );

        if (currentUses >= maxUses)
        {
            // Optional: disable interaction or play effect
            WorldPromptUI.Instance.Hide();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        if (currentUses < maxUses)
            WorldPromptUI.Instance.Show("Press E to meditate at the shrine", transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        WorldPromptUI.Instance.Hide();
    }
}