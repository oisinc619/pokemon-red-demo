using UnityEngine;

/// <summary>
/// Handles unique tracking data for an active move assigned to a specific Pokémon instance.
/// </summary>
[System.Serializable]
public class BattleMoveInstance
{
    [Header("Data Reference")]
    public BattleMoveData baseMove;

    [Header("Dynamic State")]
    public int currentPP;

    /// <summary>
    /// Constructs an active tracking move state initialized to maximum base PP capacities.
    /// </summary>
    public BattleMoveInstance(BattleMoveData moveBlueprint)
    {
        baseMove = moveBlueprint;
        currentPP = moveBlueprint.maxPP;
    }
}
