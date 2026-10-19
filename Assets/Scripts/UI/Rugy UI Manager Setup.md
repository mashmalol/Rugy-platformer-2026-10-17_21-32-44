# Rugy UI Manager Setup

## Central hierarchy

```text
Canvas
├── PausePanel
├── InventoryPanel
│   ├── InventoryGrid
│   ├── EquipmentPanel
│   └── ItemDetails
├── SettingsPanel
└── ThreadCounter
```

Add `RugyUIManager` to a persistent UI object and assign the three panels. Disable the panels in the hierarchy before Play mode. The manager handles `Escape` for pause and `I` for inventory, and pauses gameplay while either screen is open.

Do not run a second Escape-driven pause controller at the same time. If `RugyPauseMenuController` is already on the scene, remove it or set its toggle key to `None` and let `RugyUIManager` own modal state.

## Inventory screen

1. Add `RugyInventoryUI` to `InventoryPanel`.
2. Assign the player `RugyInventory` and `RugyEquipment` components.
3. Configure each grid slot with an item ID, icon, empty-state object, and Button.
4. Connect each item Button to `RugyInventoryUI.SelectItem(string)`.
5. Connect equipment buttons to `SelectEquipmentSlot(string)`.
6. Assign the `ITEM DETAILS` name and description TextMeshPro fields.

## Golden Thread counter

1. Add `RugyThreadCounterUI` beside the counter icon.
2. Assign its TextMeshPro field.
3. In `GoldenThreadCounter.onChanged`, add the object containing `RugyThreadCounterUI`.
4. Select `RugyThreadCounterUI.SetCount(int)`.

## Button wiring

```text
Resume       -> RugyUIManager.ClosePause()
Inventory    -> RugyUIManager.OpenInventory()
Close bag    -> RugyUIManager.CloseInventory()
Settings     -> RugyUIManager.OpenSettings()
Back         -> RugyUIManager.CloseSettings()
```

## Test

Press `Escape` to open and close pause. Press `I` to open and close inventory. Confirm that only one modal panel is visible and that `Time.timeScale` returns to `1` after every close action.
