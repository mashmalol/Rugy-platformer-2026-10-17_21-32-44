using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Binds the inventory and equipment data components to the generated UI screen.
/// Slot visuals are configured in the Inspector through item IDs and sprites.
/// </summary>
public class RugyInventoryUI : MonoBehaviour
{
    [System.Serializable]
    private class ItemSlot
    {
        public string itemId;
        public Image icon;
        public GameObject emptyState;
        public Button button;
    }

    [Header("Data")]
    [SerializeField] private RugyInventory inventory;
    [SerializeField] private RugyEquipment equipment;

    [Header("Inventory grid")]
    [SerializeField] private ItemSlot[] slots;

    [Header("Details")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private string defaultItemName = "SELECT AN ITEM";
    [SerializeField] private string defaultDescription = "Choose a thread, relic, or rug fragment.";

    private void OnEnable()
    {
        Refresh();
        ShowDetails(defaultItemName, defaultDescription);
    }

    public void Refresh()
    {
        if (inventory == null || slots == null) return;

        foreach (ItemSlot slot in slots)
        {
            if (slot == null) continue;
            bool owned = inventory.HasItem(slot.itemId);
            if (slot.emptyState != null) slot.emptyState.SetActive(!owned);
            if (slot.icon != null) slot.icon.enabled = owned;
            if (slot.button != null) slot.button.interactable = owned;
        }
    }

    public void SelectItem(string itemId)
    {
        if (inventory == null || !inventory.HasItem(itemId)) return;
        ShowDetails(itemId, "A strand of living pattern.");
    }

    public void SelectEquipmentSlot(string slot)
    {
        if (equipment == null) return;
        string itemId = equipment.GetEquipped(slot);
        if (string.IsNullOrEmpty(itemId))
            ShowDetails(slot.ToUpperInvariant(), "This equipment slot is empty.");
        else
            ShowDetails(itemId, "Equipped woven gear.");
    }

    private void ShowDetails(string title, string description)
    {
        if (itemNameText != null) itemNameText.text = title;
        if (itemDescriptionText != null) itemDescriptionText.text = description;
    }
}
