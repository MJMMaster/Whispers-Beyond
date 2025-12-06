using UnityEngine;

public class HeartPickUp : MonoBehaviour
{
    public string heartID;
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Inventory.Instance.AddItem(heartID);
            DialogueUIManager.Instance.Show("Player", $"Picked up {heartID}");
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