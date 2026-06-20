using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Centralized generation hub responsible for creating initialized instances of Pokémon.
/// </summary>
public static class PokemonFactory
{
    /// <summary>
    /// Generates a fully tracking individual Pokémon instance from a base species asset template.
    /// </summary>
    /// <param name="species">The template ScriptableObject asset defining base traits.</param>
    /// <param name="level">The target starting level between 1 and 100.</param>
    /// <returns>A completely configured runtime Pokémon instance.</returns>
    public static PokemonInstance CreatePokemon(PokemonSpecies species, int level)
    {
        if (species == null)
        {
            Debug.LogError("PokemonFactory: Cannot manufacture a Pokémon with a null species template reference.");
            return null;
        }

        // Initialize core instance fields, rolling unique genetic IVs and clearing EV tables
        PokemonInstance pokemon = new PokemonInstance(species, level);

        // Derive and populate the initial active level-appropriate battle moveset
        PopulateInitialMoveset(pokemon);

        return pokemon;
    }

    /// <summary>
    /// Scans the species learnset database and populates active move slots based on current level milestones.
    /// </summary>
    private static void PopulateInitialMoveset(PokemonInstance pokemon)
    {
        List<BattleMoveData> eligibleMoves = new List<BattleMoveData>();

        // Collect all moves mapped to a level less than or equal to the current instance level
        foreach (LearnableMove learnable in pokemon.species.learnset)
        {
            if (learnable.level <= pokemon.level && learnable.moveData != null)
            {
                eligibleMoves.Add(learnable.moveData);
            }
        }

        // Gen-1 Rule: If more than 4 moves are eligible, retain only the most recent 4 entries
        int moveCount = eligibleMoves.Count;
        int startingIndex = Mathf.Max(0, moveCount - 4);

        int activeSlot = 0;
        for (int i = startingIndex; i < moveCount; i++)
        {
            pokemon.moves[activeSlot] = new BattleMoveInstance(eligibleMoves[i]);
            activeSlot++;
        }
    }
}
