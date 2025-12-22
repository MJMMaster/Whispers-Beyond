using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoralityWorldTint : MonoBehaviour
{
    public static MoralityWorldTint Instance;

    [Header("References")]
    public Image tintImage;

    [Header("Colors")]
    public Color negativeColor = new Color(0.7f, 0f, 0f, 0.6f);
    public Color positiveColor = new Color(1f, 0.85f, 0.4f, 0.5f);

    [Header("Intensity Curve")]
    public AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Settings")]
    public float transitionSpeed = 5f;

    private Color targetColor = Color.clear;

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

        tintImage.color = Color.clear;
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Attempt to relink the image if lost
        if (tintImage == null)
            tintImage = FindObjectOfType<Canvas>().GetComponentInChildren<Image>();

        // Immediately refresh color
        if (MoralityManager.Instance != null)
            UpdateTint(MoralityManager.Instance.GetMorality());
    }

    private void Start()
    {
        if (MoralityManager.Instance != null)
        {
            MoralityManager.Instance.OnMoralityChanged += UpdateTint;
            UpdateTint(MoralityManager.Instance.GetMorality());
        }
    }

    private void OnDestroy()
    {
        if (MoralityManager.Instance != null)
            MoralityManager.Instance.OnMoralityChanged -= UpdateTint;
    }

    private void Update()
    {
        if (tintImage != null)
            tintImage.color = Color.Lerp(tintImage.color, targetColor, Time.deltaTime * transitionSpeed);
    }

    private void UpdateTint(int morality)
    {
        if (morality == 0)
        {
            targetColor = Color.clear;
            return;
        }

        float normalized;
        if (morality < 0)
        {
            normalized = Mathf.InverseLerp(0, MoralityManager.Instance.minMorality, morality);
            targetColor = new Color(negativeColor.r, negativeColor.g, negativeColor.b, negativeColor.a * intensityCurve.Evaluate(normalized));
        }
        else
        {
            normalized = Mathf.InverseLerp(0, MoralityManager.Instance.maxMorality, morality);
            targetColor = new Color(positiveColor.r, positiveColor.g, positiveColor.b, positiveColor.a * intensityCurve.Evaluate(normalized));
        }
    }
}