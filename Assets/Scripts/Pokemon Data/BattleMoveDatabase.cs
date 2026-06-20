using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central indexed container providing global read-access to all battle move templates.
/// </summary>
public class BattleMoveDatabase : MonoBehaviour
{
    // Global static reference allowing any script to easily read data from the registry
    public static BattleMoveDatabase Instance { get; private set; }

    [Header("Move Registry")]
    [SerializeField] private List<BattleMoveData> allMoves = new List<BattleMoveData>();

    // Internal quick-lookup table sorting move blueprints by their exact string names
    private readonly Dictionary<string, BattleMoveData> moveLookupTable = new Dictionary<string, BattleMoveData>();

    private void Awake()
    {
        // Enforce the Singleton pattern to ensure only one instance of the database exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeLookupTable();
    }

    /// <summary>
    /// Builds the internal dictionary lookup keys from the assigned ScriptableObject list.
    /// </summary>
    private void InitializeLookupTable()
    {
        moveLookupTable.Clear();

        foreach (BattleMoveData move in allMoves)
        {
            if (move == null) continue;

            // Normalize names to lowercase to ensure lookup operations are case-insensitive
            string key = move.moveName.ToLower().Trim();

            if (!moveLookupTable.ContainsKey(key))
            {
                moveLookupTable.Add(key, move);
            }
            else
            {
                Debug.LogWarning($"BattleMoveDatabase: Duplicate move entry detected for name '{move.moveName}'. Skipping duplicate.");
            }
        }
    }

    /// <summary>
    /// Retrieves a battle move template directly via its case-insensitive string name.
    /// </summary>
    /// <param name="moveName">The exact name of the move to search for.</param>
    /// <returns>The matching BattleMoveData asset, or null if not registered.</returns>
    public BattleMoveData GetMoveByName(string moveName)
    {
        if (string.IsNullOrEmpty(moveName)) return null;

        string key = moveName.ToLower().Trim();

        if (moveLookupTable.TryGetValue(key, out BattleMoveData move))
        {
            return move;
        }

        Debug.LogError($"BattleMoveDatabase: Request failed. Move name '{moveName}' is not registered in the database list.");
        return null;
    }
}
