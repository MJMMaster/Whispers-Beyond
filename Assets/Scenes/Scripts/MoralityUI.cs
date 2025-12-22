using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoralityUI : MonoBehaviour
{
    public static MoralityUI Instance;
    public TMP_Text moralityText;

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
        if (MoralityManager.Instance != null)
        {
            MoralityManager.Instance.OnMoralityChanged += UpdateMorality;
            UpdateMorality(MoralityManager.Instance.GetMorality());
        }
    }

    private void OnDestroy()
    {
        if (MoralityManager.Instance != null)
            MoralityManager.Instance.OnMoralityChanged -= UpdateMorality;
    }

    private void UpdateMorality(int newValue)
    {
        moralityText.text = "Morality: " + newValue;
    }
}