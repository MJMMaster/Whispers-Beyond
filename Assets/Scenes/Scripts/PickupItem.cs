using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Pickup Settings")]
    public string itemID;
    public string pickupMessage = "Picked up";

    private bool playerInRange = false;

    private void Start()
    {
        // Destroy immediately if already collected
        if (Inventory.Instance != null && Inventory.Instance.IsCollected(itemID))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Inventory.Instance.AddItem(itemID);
            Inventory.Instance.MarkCollected(itemID);

            // Show pickup message via persistent UI
            PickupUIManager.Instance?.ShowPickupMessage($"{pickupMessage} {itemID}");

            WorldPromptUI.Instance.Hide();

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        WorldPromptUI.Instance.Show("Press E to pick up", transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        WorldPromptUI.Instance.Hide();
    }
}