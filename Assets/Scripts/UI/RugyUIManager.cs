using UnityEngine;

/// <summary>
/// Coordinates Rugy's pause, inventory, and settings panels.
/// Only one modal panel is visible at a time and gameplay is paused while
/// pause or inventory screens are open.
/// </summary>
public class RugyUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Input")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private KeyCode inventoryKey = KeyCode.I;

    private bool modalOpen;

    public bool IsModalOpen => modalOpen;

    private void Start()
    {
        CloseAllPanels();
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (settingsPanel != null && settingsPanel.activeSelf)
                CloseSettings();
            else if (inventoryPanel != null && inventoryPanel.activeSelf)
                CloseInventory();
            else
                TogglePause();
        }

        if (Input.GetKeyDown(inventoryKey))
        {
            if (inventoryPanel != null && inventoryPanel.activeSelf)
                CloseInventory();
            else
                OpenInventory();
        }
    }

    public void TogglePause()
    {
        if (pausePanel != null && pausePanel.activeSelf)
            ClosePause();
        else
            OpenPause();
    }

    public void OpenPause()
    {
        CloseNonPausePanels();
        if (pausePanel != null) pausePanel.SetActive(true);
        SetModalState(true);
    }

    public void ClosePause()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        SetModalState(false);
    }

    public void OpenInventory()
    {
        CloseNonInventoryPanels();
        if (inventoryPanel != null) inventoryPanel.SetActive(true);
        SetModalState(true);
    }

    public void CloseInventory()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        SetModalState(false);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
        SetModalState(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        SetModalState(pausePanel != null && pausePanel.activeSelf);
    }

    public void CloseAllPanels()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        SetModalState(false);
    }

    private void CloseNonPausePanels()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    private void CloseNonInventoryPanels()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    private void SetModalState(bool open)
    {
        modalOpen = open;
        Time.timeScale = open ? 0f : 1f;
        Cursor.visible = open;
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
