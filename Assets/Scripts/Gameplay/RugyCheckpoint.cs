using UnityEngine;

/// <summary>Golden Tassel checkpoint that records the player's respawn location.</summary>
public class RugyCheckpoint : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    private bool activated;

    private void OnTriggerEnter(Collider other)
    {
        RugyRespawn respawn = other.GetComponentInParent<RugyRespawn>();
        if (respawn == null) return;
        respawn.SetCheckpoint(respawnPoint != null ? respawnPoint : transform);
        activated = true;
    }

    public bool IsActivated => activated;
}
