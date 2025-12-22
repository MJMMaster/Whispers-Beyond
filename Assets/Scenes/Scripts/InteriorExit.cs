using UnityEngine;
using UnityEngine.SceneManagement;

public class InteriorExit : MonoBehaviour
{
    public string returnSceneName;
    private bool playerInRange;

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(returnSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        WorldPromptUI.Instance.Show("Press E to Leave", transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        WorldPromptUI.Instance.Hide();
    }
}