using UnityEngine;
using UnityEngine.Events;

/// <summary>Reusable health component for players, enemies, and bosses.</summary>
public class RugyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private bool resetOnEnable = true;
    [SerializeField] private UnityEvent onDamaged;
    [SerializeField] private UnityEvent onDeath;

    private float currentHealth;
    private bool dead;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => dead;
    public UnityEvent Damaged => onDamaged;
    public UnityEvent Died => onDeath;

    private void Awake()
    {
        ResetHealth();
    }

    private void OnEnable()
    {
        if (resetOnEnable && Application.isPlaying)
            ResetHealth();
    }

    public void TakeDamage(float amount)
    {
        if (dead || amount <= 0f)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        onDamaged?.Invoke();

        if (currentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (dead || amount <= 0f)
            return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        dead = false;
    }

    private void Die()
    {
        if (dead)
            return;

        dead = true;
        onDeath?.Invoke();
    }
}
