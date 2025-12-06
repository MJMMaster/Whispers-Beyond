using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    [HideInInspector]
    public QuestSO activeQuest;

    private NPC nearbyNPC;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Interact with NPC
        if (nearbyNPC != null && Input.GetKeyDown(KeyCode.E))
        {
            nearbyNPC.Interact(activeQuest);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        NPC npc = other.GetComponent<NPC>();
        if (npc != null)
        {
            nearbyNPC = npc;

            if (activeQuest != null &&
                activeQuest.questType == QuestSO.QuestType.Closure &&
                activeQuest.requiredNPCID == npc.npcID)
            {
                Debug.Log($"Press E to complete quest with {npc.npcName}.");
            }
            else
            {
                Debug.Log($"Press E to talk to {npc.npcName}.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        NPC npc = other.GetComponent<NPC>();
        if (npc != null && npc == nearbyNPC)
        {
            nearbyNPC = null;
        }
    }
}