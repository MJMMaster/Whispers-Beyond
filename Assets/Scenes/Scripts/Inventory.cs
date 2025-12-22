using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private List<string> items = new List<string>();
    private HashSet<string> collectedItems = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemID)
    {
        if (!items.Contains(itemID))
        {
            items.Add(itemID);
            Debug.Log("Added: " + itemID);
        }
    }

    public bool HasItem(string itemID)
    {
        return items.Contains(itemID);
    }

    public void RemoveItem(string itemID)
    {
        if (items.Contains(itemID))
        {
            items.Remove(itemID);
            Debug.Log("Removed: " + itemID);
        }
    }

    // Mark item as collected
    public void MarkCollected(string itemID)
    {
        collectedItems.Add(itemID);
    }

    // Check if item has been collected already
    public bool IsCollected(string itemID)
    {
        return collectedItems.Contains(itemID);
    }
}