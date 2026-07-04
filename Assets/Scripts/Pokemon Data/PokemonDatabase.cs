using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central registry asset serving as a global lookup index for all Pokémon species in the game.
/// Lives as a permanent file within the project directories.
/// </summary>
[CreateAssetMenu(fileName = "PokemonDatabase", menuName = "Pokemon/Pokemon Database")]
public class PokemonDatabase : ScriptableObject
{
    [Header("Master Species Index")]
    [SerializeField]
    private List<PokemonSpecies> speciesList = new List<PokemonSpecies>();

    private Dictionary<int, PokemonSpecies> speciesLookupTable;

    /// <summary>
    /// Builds the internal dictionary lookup keys from the assigned ScriptableObject list.
    /// Run this from your global GameSystems hub during the early game boot sequence.
    /// </summary>
    public void InitializeLookupTable()
    {
        speciesLookupTable = new Dictionary<int, PokemonSpecies>();

        foreach (var species in speciesList)
        {
            if (species == null) continue;

            if (!speciesLookupTable.ContainsKey(species.speciesId))
            {
                speciesLookupTable.Add(species.speciesId, species);
            }
            else
            {
                Debug.LogWarning($"PokemonDatabase: Duplicate species entry detected for ID '{species.speciesId}' ({species.speciesName}). Skipping duplicate.");
            }
        }
    }

    /// <summary>
    /// Retrieves a species static blueprint using its distinct identifier integer.
    /// </summary>
    /// <param name="id">The unique ID of the target species.</param>
    /// <returns>The matching PokemonSpecies blueprint asset, or null if not found.</returns>
    public PokemonSpecies GetSpeciesById(int id)
    {
        if (speciesLookupTable == null)
        {
            InitializeLookupTable();
        }

        if (speciesLookupTable.TryGetValue(id, out PokemonSpecies matchedSpecies))
        {
            return matchedSpecies;
        }

        Debug.LogError($"PokemonDatabase: Request failed. Species ID '{id}' is not registered in the database list.");
        return null;
    }
}
