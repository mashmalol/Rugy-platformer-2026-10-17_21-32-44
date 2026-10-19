using UnityEngine;
using UnityEngine.Events;

/// <summary>Minimal boss lifecycle controller for intro, active fight, and defeat.</summary>
public class RugyBossController : MonoBehaviour
{
    [SerializeField] private RugyHealth health;
    [SerializeField] private GameObject arenaLock;
    [SerializeField] private UnityEvent onIntro;
    [SerializeField] private UnityEvent onDefeated;
    private bool active;

    private void Awake()
    {
        if (health == null) health = GetComponent<RugyHealth>();
        if (health != null) health.Died.AddListener(Defeat);
        if (arenaLock != null) arenaLock.SetActive(false);
    }

    public void BeginEncounter()
    {
        active = true;
        if (arenaLock != null) arenaLock.SetActive(true);
        onIntro?.Invoke();
    }

    private void Defeat()
    {
        if (!active) return;
        active = false;
        if (arenaLock != null) arenaLock.SetActive(false);
        onDefeated?.Invoke();
    }
}
