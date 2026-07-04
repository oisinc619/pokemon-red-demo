using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central registry asset serving as a global lookup index for all items in the game.
/// Lives as a permanent file within the project directories.
/// </summary>
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Items/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [Header("Item Catalog")]
    [SerializeField]
    private List<ItemData> itemsList = new List<ItemData>();

    private Dictionary<int, ItemData> itemLookupCache;

    /// <summary>
    /// Initializes the fast lookup index table for O(1) performance.
    /// Run this from your global GameSystems hub during the early game boot sequence.
    /// </summary>
    public void InitializeDatabase()
    {
        itemLookupCache = new Dictionary<int, ItemData>();

        foreach (var item in itemsList)
        {
            if (item == null) continue;

            if (!itemLookupCache.ContainsKey(item.itemId))
            {
                itemLookupCache.Add(item.itemId, item);
            }
            else
            {
                Debug.LogWarning($"ItemDatabase: Duplicate item entry detected for ID '{item.itemId}' ({item.itemName}). Skipping duplicate.");
            }
        }
    }

    /// <summary>
    /// Retrieves an item blueprint using its distinct identifier integer.
    /// </summary>
    /// <param name="id">The unique ID of the target item.</param>
    /// <returns>The matching ItemData blueprint asset, or null if not found.</returns>
    public ItemData GetItemById(int id)
    {
        if (itemLookupCache == null)
        {
            InitializeDatabase();
        }

        if (itemLookupCache.TryGetValue(id, out ItemData matchedItem))
        {
            return matchedItem;
        }

        Debug.LogError($"ItemDatabase: Request failed. Item ID '{id}' is not registered in the database list.");
        return null;
    }
}
