using UnityEngine;
using System;

public class MoralityManager : MonoBehaviour
{
    public static MoralityManager Instance;

    [Header("Player Morality")]
    public int currentMorality = 0;

    // Event that UI listens to
    public Action<int> OnMoralityChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ModifyMorality(int amount)
    {
        currentMorality += amount;

        // Notify UI
        OnMoralityChanged?.Invoke(currentMorality);

        Debug.Log("Morality Changed: " + currentMorality);
    }
}