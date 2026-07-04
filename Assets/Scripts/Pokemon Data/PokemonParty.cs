using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the collection of Pokémon currently carried by the player or a trainer.
/// </summary>
public class PokemonParty : MonoBehaviour
{
    private const int MAX_PARTY_SIZE = 6;

    [SerializeField]
    private List<PokemonInstance> partyMembers = new List<PokemonInstance>();

    /// <summary>
    /// Gets a read-only list of the current party members.
    /// </summary>
    public IReadOnlyList<PokemonInstance> PartyMembers => partyMembers;

    /// <summary>
    /// Attempts to add a new Pokémon instance to the party.
    /// </summary>
    /// <param name="newPokemon">The initialized Pokémon instance to add.</param>
    /// <returns>True if successfully added; false if the party is full.</returns>
    public bool AddPokemon(PokemonInstance newPokemon)
    {
        if (partyMembers.Count >= MAX_PARTY_SIZE)
        {
            return false;
        }

        partyMembers.Add(newPokemon);
        return true;
    }

    /// <summary>
    /// Checks if at least one Pokémon in the party is capable of fighting.
    /// </summary>
    /// <returns>True if any member has current HP above zero.</returns>
    public bool HasUsablePokemon()
    {
        foreach (var pokemon in partyMembers)
        {
            // Changed from CurrentHP to currentHP to match your instance variables
            if (pokemon.currentHP > 0)
            {
                return true;
            }
        }
        return false;
    }

}
