using UnityEngine;
using UnityEngine.Events;

/// <summary>Minimal inventory container for item IDs used by the equipment UI.</summary>
public class RugyInventory : MonoBehaviour
{
    [SerializeField] private string[] itemIds = new string[0];
    [SerializeField] private UnityEvent onInventoryChanged;

    public bool HasItem(string itemId)
    {
        if (string.IsNullOrEmpty(itemId)) return false;
        foreach (string id in itemIds) if (id == itemId) return true;
        return false;
    }

    public void AddItem(string itemId)
    {
        if (string.IsNullOrEmpty(itemId) || HasItem(itemId)) return;
        string[] next = new string[itemIds.Length + 1];
        itemIds.CopyTo(next, 0);
        next[next.Length - 1] = itemId;
        itemIds = next;
        onInventoryChanged?.Invoke();
    }
}
