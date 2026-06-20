using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static database template defining the immutable properties of a specific Pokémon species.
/// </summary>
[CreateAssetMenu(fileName = "New Species", menuName = "Pokemon/Species Blueprint")]
public class PokemonSpecies : ScriptableObject
{
    [Header("Identity Data")]
    public int indexNumber;
    public string speciesName;
    public PokemonType primaryType;
    public PokemonType secondaryType;

    [Header("Base Stats (Generation I)")]
    public int baseHP;
    public int baseAttack;
    public int baseDefense;
    public int baseSpecial;
    public int baseSpeed;

    [Header("Growth & Progression")]
    public GrowthRate growthRate;
    public int baseExpYield;

    [Header("Evolution Mapping")]
    public EvolutionType evolutionType;
    public int evolutionLevel;
    public PokemonSpecies evolutionTarget;

    [Header("Move Learning Roadmap")]
    public List<LearnableMove> learnset = new List<LearnableMove>();
}

/// <summary>
/// Dictates the mechanical condition required to trigger a species metamorphosis event.
/// </summary>
public enum EvolutionType
{
    None,
    LevelUp,
    Item,
    Trade
}

public enum PokemonType
{
    Normal, Fire, Water, Grass, Electric, Ice, Fighting,
    Poison, Ground, Flying, Psychic, Bug, Rock, Ghost, Dragon
}

public enum GrowthRate
{
    Fast, MediumFast, MediumSlow, Slow
}

/// <summary>
/// Configurable database struct pairing a target learning level checkpoint with a specific battle move blueprint.
/// </summary>
[System.Serializable]
public struct LearnableMove
{
    public int level;
    public BattleMoveData moveData;
}
