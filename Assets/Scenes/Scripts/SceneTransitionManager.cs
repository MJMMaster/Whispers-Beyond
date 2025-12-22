using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    private string pendingSpawnTag = "PlayerSpawn";

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(string sceneName, string spawnTag)
    {
        pendingSpawnTag = spawnTag;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
            if (string.IsNullOrEmpty(pendingSpawnTag))
                return;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("SceneTransitionManager: Player not found.");
                return;
            }

            // SAFELY check tag existence
            GameObject spawnPoint = null;
            try
            {
                spawnPoint = GameObject.FindGameObjectWithTag(pendingSpawnTag);
            }
            catch
            {
                Debug.LogWarning($"Spawn tag '{pendingSpawnTag}' is not defined.");
                return;
            }

            if (spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;
            }
            else
            {
                Debug.LogWarning($"No object found with tag '{pendingSpawnTag}'.");
            }
        StartCoroutine(PlacePlayerNextFrame());
    }

    private IEnumerator PlacePlayerNextFrame()
    {
        yield return null; // wait one frame

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("SceneTransitionManager: Player not found!");
            yield break;
        }

        if (!TagExists(pendingSpawnTag))
        {
            Debug.LogError($"Spawn tag '{pendingSpawnTag}' does not exist!");
            yield break;
        }

        GameObject spawnPoint = GameObject.FindGameObjectWithTag(pendingSpawnTag);
        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogWarning($"Spawn point with tag {pendingSpawnTag} not found in scene.");
        }
    }

    private bool TagExists(string tag)
    {
        try
        {
            GameObject.FindGameObjectWithTag(tag);
            return true;
        }
        catch
        {
            return false;
        }
    }
}