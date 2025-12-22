using UnityEngine;
using TMPro;
using System.Collections;

public class PickupUIManager : MonoBehaviour
{
    public static PickupUIManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI pickupText; // Assign in inspector

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes

            if (pickupText != null)
                pickupText.gameObject.SetActive(false); // Hide initially
            else
                Debug.LogWarning("PickupUIManager: PickupText not assigned!");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Show a pickup message for a duration, fading out
    /// </summary>
    public void ShowPickupMessage(string message, float duration = 2f)
    {
        if (pickupText == null) return;

        StartCoroutine(ShowTextCoroutine(message, duration));
    }

    private IEnumerator ShowTextCoroutine(string message, float duration)
    {
        pickupText.text = message;
        pickupText.alpha = 1f;
        pickupText.gameObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            pickupText.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }

        pickupText.gameObject.SetActive(false);
    }
}