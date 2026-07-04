using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central registry asset serving as a global lookup index for all battle moves in the game.
/// Lives as a permanent file within the project directories.
/// </summary>
[CreateAssetMenu(fileName = "BattleMoveDatabase", menuName = "Pokemon/Battle Move Database")]
public class BattleMoveDatabase : ScriptableObject
{
    [Header("Move Registry")]
    [SerializeField]
    private List<BattleMoveData> allMoves = new List<BattleMoveData>();

    private Dictionary<string, BattleMoveData> moveLookupTable;

    /// <summary>
    /// Builds the internal dictionary lookup keys from the assigned ScriptableObject list.
    /// Run this from your global GameSystems hub during the early game boot sequence.
    /// </summary>
    public void InitializeLookupTable()
    {
        moveLookupTable = new Dictionary<string, BattleMoveData>();

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
        if (moveLookupTable == null)
        {
            InitializeLookupTable();
        }

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
