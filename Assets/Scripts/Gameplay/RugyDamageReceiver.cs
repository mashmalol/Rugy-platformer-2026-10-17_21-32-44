using UnityEngine;

/// <summary>Forwards incoming damage to a RugyHealth component.</summary>
public class RugyDamageReceiver : MonoBehaviour
{
    [SerializeField] private RugyHealth health;
    [SerializeField] private float invulnerabilityTime = 0.15f;
    private float invulnerableUntil;

    private void Awake() { if (health == null) health = GetComponentInParent<RugyHealth>(); }

    public void ReceiveDamage(float amount)
    {
        if (Time.time < invulnerableUntil || health == null || health.IsDead) return;
        invulnerableUntil = Time.time + invulnerabilityTime;
        health.TakeDamage(amount);
    }
}
