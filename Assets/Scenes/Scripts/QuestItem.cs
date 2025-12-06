using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public string itemID;
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Inventory.Instance.AddItem(itemID);
            Debug.Log($"Picked up {itemID}");
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