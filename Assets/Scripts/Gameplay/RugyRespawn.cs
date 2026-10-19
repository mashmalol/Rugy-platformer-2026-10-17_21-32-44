using UnityEngine;

/// <summary>Respawns a player at the latest checkpoint after death or falling.</summary>
public class RugyRespawn : MonoBehaviour
{
    [SerializeField] private RugyHealth health;
    [SerializeField] private float fallY = -12f;
    [SerializeField] private float respawnDelay = 1f;
    private Vector3 checkpointPosition;
    private Quaternion checkpointRotation;
    private bool respawning;

    private void Awake()
    {
        if (health == null) health = GetComponent<RugyHealth>();
        checkpointPosition = transform.position;
        checkpointRotation = transform.rotation;
        if (health != null) health.Died.AddListener(BeginRespawn);
    }

    private void Update()
    {
        if (!respawning && transform.position.y < fallY) BeginRespawn();
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        if (checkpoint == null) return;
        checkpointPosition = checkpoint.position;
        checkpointRotation = checkpoint.rotation;
    }

    public void BeginRespawn()
    {
        if (!respawning) Invoke(nameof(Respawn), respawnDelay);
        respawning = true;
    }

    private void Respawn()
    {
        transform.SetPositionAndRotation(checkpointPosition, checkpointRotation);
        if (health != null) health.ResetHealth();
        respawning = false;
    }
}
