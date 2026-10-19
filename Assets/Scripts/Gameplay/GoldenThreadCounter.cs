using UnityEngine;
using UnityEngine.Events;

/// <summary>Stores the player's Golden Thread count for UI and inventory systems.</summary>
public class GoldenThreadCounter : MonoBehaviour
{
    [SerializeField] private int count;
    [SerializeField] private UnityEvent<int> onChanged;
    public int Count => count;

    public void Add(int amount)
    {
        if (amount <= 0) return;
        count += amount;
        onChanged?.Invoke(count);
    }

    public bool Spend(int amount)
    {
        if (amount <= 0 || count < amount) return false;
        count -= amount;
        onChanged?.Invoke(count);
        return true;
    }
}
