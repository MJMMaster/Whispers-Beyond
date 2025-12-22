using UnityEngine;

public class Ghost : MonoBehaviour
{
    public string ghostName = "Ghost";

    private bool playerInRange = false;

   

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        WorldPromptUI.Instance.Hide();
    }
}