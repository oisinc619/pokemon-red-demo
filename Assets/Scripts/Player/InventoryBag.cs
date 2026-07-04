using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pairs a specific item blueprint asset with an active integer tracking quantity.
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int count;

    public InventorySlot(ItemData item, int count)
    {
        this.item = item;
        this.count = count;
    }
}

/// <summary>
/// Manages the player's carrying inventory bag capacity limits and stacking logic.
/// </summary>
public class InventoryBag : MonoBehaviour
{
    private const int MAX_BAG_SLOTS = 40;
    private const int MAX_STACK_SIZE = 99;

    [SerializeField]
    private List<InventorySlot> slots = new List<InventorySlot>();

    /// <summary>
    /// Read-only access exposing current items in the bag for UI layout screens.
    /// </summary>
    public IReadOnlyList<InventorySlot> Slots => slots;

    /// <summary>
    /// Attempts to add an item item bundle to the bag inventory data model.
    /// </summary>
    /// <returns>True if successfully added or stacked; false if the bag is completely full.</returns>
    public bool TryAddItem(ItemData item, int countToAdd = 1)
    {
        // 1. First Pass: Check if the item already exists and has room to grow its stack size
        foreach (var slot in slots)
        {
            if (slot.item.itemId == item.itemId && slot.count < MAX_STACK_SIZE)
            {
                int roomInStack = MAX_STACK_SIZE - slot.count;
                int amountToStack = Mathf.Min(countToAdd, roomInStack);

                slot.count += amountToStack;
                countToAdd -= amountToStack;

                if (countToAdd <= 0) return true; // Whole bundle packed successfully
            }
        }

        // 2. Second Pass: If remainder remains, check if we have an open grid slot for a new item
        while (countToAdd > 0)
        {
            if (slots.Count >= MAX_BAG_SLOTS)
            {
                Debug.LogWarning($"InventoryBag: Failed to add item '{item.itemName}'. The pocket is full!");
                return false; // Reached physical bag constraint barrier limit
            }

            int currentStackAmount = Mathf.Min(countToAdd, MAX_STACK_SIZE);
            slots.Add(new InventorySlot(item, currentStackAmount));
            countToAdd -= currentStackAmount;
        }

        return true;
    }
}
