using UnityEngine;
using System;

public class MoralityManager : MonoBehaviour
{
    public static MoralityManager Instance;

    [Header("Morality Settings")]
    public int minMorality = -100;
    public int maxMorality = 100;

    [Header("Current Morality")]
    [SerializeField] public int currentMorality = 0;

    public event Action<int> OnMoralityChanged;

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
            return;
        }
    }

    private void Start()
    {
        OnMoralityChanged?.Invoke(currentMorality);
    }

    public void ModifyMorality(int amount)
    {
        currentMorality = Mathf.Clamp(
            currentMorality + amount,
            minMorality,
            maxMorality
        );

        OnMoralityChanged?.Invoke(currentMorality);
        Debug.Log($"Morality Changed: {currentMorality}");
    }

    public int GetMorality() => currentMorality;
}