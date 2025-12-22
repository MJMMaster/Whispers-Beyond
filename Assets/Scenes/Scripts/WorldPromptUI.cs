using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WorldPromptUI : MonoBehaviour
{
    public static WorldPromptUI Instance;

    [Header("UI Prefab")]
    public GameObject promptPrefab; // Assign prefab in inspector
    private GameObject promptObject;
    private TextMeshProUGUI promptText;

    private Transform currentTarget;
    private string currentPromptText = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (promptPrefab != null)
            {
                // Instantiate prefab at world origin
                promptObject = Instantiate(promptPrefab);
                promptText = promptObject.GetComponentInChildren<TextMeshProUGUI>();

                if (promptText == null)
                    Debug.LogError("WorldPromptUI: TMP component not found in prefab!");

                // Make the prompt itself persistent
                DontDestroyOnLoad(promptObject);

                promptObject.SetActive(false);
            }
            else
            {
                Debug.LogError("WorldPromptUI: Prompt Prefab not assigned!");
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void Update()
    {
        if (promptObject == null || !promptObject.activeSelf) return;

        if (currentTarget == null)
        {
            Hide();
            return;
        }

        // Smooth follow
        promptObject.transform.position = currentTarget.position + Vector3.up * 1f;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If prompt was active before scene change, reassign the target
        if (!string.IsNullOrEmpty(currentPromptText))
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                currentTarget = playerObj.transform;
                promptText.text = currentPromptText;
                promptObject.SetActive(true);
            }
            else
            {
                promptObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Show a prompt above a target
    /// </summary>
    public void Show(string text, Transform followTarget)
    {
        if (promptObject == null || followTarget == null) return;

        currentTarget = followTarget;
        currentPromptText = text;
        promptText.text = text;
        promptObject.SetActive(true);
    }

    /// <summary>
    /// Hide the prompt
    /// </summary>
    public void Hide()
    {
        currentTarget = null;
        currentPromptText = "";
        if (promptObject != null)
            promptObject.SetActive(false);
    }
}