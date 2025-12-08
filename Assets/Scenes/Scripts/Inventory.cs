using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private List<string> items = new List<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(string itemID)
    {
        items.Add(itemID);
        Debug.Log("Added: " + itemID);
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
}