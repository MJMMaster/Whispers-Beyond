using UnityEngine;

public class WorldPromptUI : MonoBehaviour
{
    public static WorldPromptUI Instance;
    public GameObject promptObject;
    public TMPro.TextMeshProUGUI promptText;

    private void Awake()
    {
        Instance = this;
        promptObject.SetActive(false);
    }

    public void Show(string text, Transform followTarget)
    {
        promptObject.SetActive(true);
        promptText.text = text;

        // stick above object
        promptObject.transform.position = followTarget.position + Vector3.up * 1f;
    }

    public void Hide()
    {
        promptObject.SetActive(false);
    }
}