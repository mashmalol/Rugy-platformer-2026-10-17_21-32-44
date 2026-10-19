using UnityEngine;
using UnityEngine.UI;

/// <summary>Binds a RugyHealth component to a filled Unity UI Image.</summary>
public class RugyBossHealthUI : MonoBehaviour
{
    [SerializeField] private RugyHealth health;
    [SerializeField] private Image fill;

    private void Update()
    {
        if (health == null || fill == null || health.MaxHealth <= 0f) return;
        fill.fillAmount = health.CurrentHealth / health.MaxHealth;
    }
}
