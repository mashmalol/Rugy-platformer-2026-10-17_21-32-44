using UnityEngine;

/// <summary>Persistent run-state manager for scene transitions and completion.</summary>
public class RugyGameManager : MonoBehaviour
{
    public static RugyGameManager Instance { get; private set; }
    public bool LevelComplete { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CompleteLevel()
    {
        LevelComplete = true;
    }

    public void ResetRun()
    {
        LevelComplete = false;
    }
}
