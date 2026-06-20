using UnityEngine;

/// <summary>
/// Static template defining the immutable properties of a combat attack move.
/// </summary>
[CreateAssetMenu(fileName = "New Battle Move", menuName = "Pokemon/Battle Move Blueprint")]
public class BattleMoveData : ScriptableObject
{
    [Header("Identity")]
    public string moveName;
    public PokemonType type;

    [Header("Combat Variables")]
    public MoveCategory category;
    public int basePower;
    public int accuracy;
    public int maxPP;
}

public enum MoveCategory
{
    Physical,
    Special,
    Status
}
