using UnityEngine;

public class SpiritSight : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.Space;
    private bool spiritSightActive = false;

    void Start()
    {
        // Make all ghosts invisible at the beginning
        ToggleSpiritObjects(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            spiritSightActive = !spiritSightActive;
            ToggleSpiritObjects(spiritSightActive);
        }
    }

    void ToggleSpiritObjects(bool active)
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in ghosts)
        {
            SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = active;
            }
        }
    }
}