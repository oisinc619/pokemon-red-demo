using UnityEngine;

/// <summary>
/// Immutable blueprint definition for an in-game inventory item.
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item Blueprint")]
public class ItemData : ScriptableObject
{
    [Header("Identity")]
    public int itemId;
    public string itemName;

    [TextArea(2, 4)]
    public string itemDescription;

    [Header("Categorization")]
    public ItemCategory category;
    public int price;

    [Header("Mechanical Parameters")]
    [Tooltip("Value used for healing, capture modifiers, or specific engine effects.")]
    public int executionValue;
}

public enum ItemCategory
{
    General,
    Recovery,
    Ball,
    KeyItem
}
