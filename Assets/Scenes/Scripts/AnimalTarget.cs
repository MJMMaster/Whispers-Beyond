using UnityEngine;

public class AnimalTarget : MonoBehaviour
{
    public GameObject heartPrefab;
    public Transform dropPoint;
    public string heartID = "animal_heart";

    private bool playerInside = false;
    private bool hasDropped = false;

    private void Update()
    {
        if (playerInside && !hasDropped && Input.GetKeyDown(KeyCode.E))
        {
            KillAnimal();
        }
    }

    private void KillAnimal()
    {
        hasDropped = true;

        DialogueUIManager.Instance.Show("Player", "You killed an animal.");

        GameObject droppedHeart = Instantiate(heartPrefab, dropPoint.position, Quaternion.identity);
        droppedHeart.GetComponent<HeartPickUp>().heartID = heartID;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        WorldPromptUI.Instance.Show("Press E to kill animal", transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        WorldPromptUI.Instance.Hide();
    }
}