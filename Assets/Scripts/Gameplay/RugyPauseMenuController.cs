using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls Rugy's pause-menu skeleton.
/// Toggles a menu root, pauses time, and exposes button-friendly actions.
/// </summary>
public class RugyPauseMenuController : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject menuRoot;
    [SerializeField] private KeyCode toggleKey = KeyCode.Escape;
    [SerializeField] private bool pauseOnStart;

    [Header("Optional panels")]
    [SerializeField] private GameObject settingsPanel;

    private bool isPaused;
    private string activeSceneName;

    public bool IsPaused => isPaused;

    private void Awake()
    {
        activeSceneName = SceneManager.GetActiveScene().name;
        SetPaused(pauseOnStart);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            SetPaused(!isPaused);
    }

    /// <summary>
    /// Opens or closes the pause menu and synchronizes Unity's time scale.
    /// </summary>
    public void SetPaused(bool paused)
    {
        isPaused = paused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (menuRoot != null)
            menuRoot.SetActive(isPaused);

        if (!isPaused && settingsPanel != null)
            settingsPanel.SetActive(false);

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    /// <summary>Button action for Resume.</summary>
    public void Resume()
    {
        SetPaused(false);
    }

    /// <summary>Reloads the current scene after restoring normal time.</summary>
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(activeSceneName);
    }

    /// <summary>Button action for opening the settings panel.</summary>
    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    /// <summary>Button action for closing the settings panel.</summary>
    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    /// <summary>Returns to the first scene in the build settings.</summary>
    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
