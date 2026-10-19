using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>Stores equipped item IDs by slot for the inventory and equipment UI.</summary>
public class RugyEquipment : MonoBehaviour
{
    [SerializeField] private UnityEvent onEquipmentChanged;
    private readonly Dictionary<string, string> equipped = new Dictionary<string, string>();

    public string GetEquipped(string slot)
    {
        if (string.IsNullOrEmpty(slot)) return string.Empty;
        return equipped.TryGetValue(slot, out string item) ? item : string.Empty;
    }

    public void Equip(string slot, string itemId)
    {
        if (string.IsNullOrEmpty(slot) || string.IsNullOrEmpty(itemId)) return;
        equipped[slot] = itemId;
        onEquipmentChanged?.Invoke();
    }

    public void Unequip(string slot)
    {
        if (string.IsNullOrEmpty(slot)) return;
        if (equipped.Remove(slot)) onEquipmentChanged?.Invoke();
    }
}
