using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central indexed container providing global read-access to all Pokémon species templates.
/// </summary>
public class PokemonDatabase : MonoBehaviour
{
    // Global static reference allowing any script to easily read data from the registry
    public static PokemonDatabase Instance { get; private set; }

    [Header("Species Registry")]
    [SerializeField] private List<PokemonSpecies> allSpecies = new List<PokemonSpecies>();

    // Internal quick-lookup table sorting species by their standard Index Number IDs
    private readonly Dictionary<int, PokemonSpecies> speciesLookupTable = new Dictionary<int, PokemonSpecies>();

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
        speciesLookupTable.Clear();

        foreach (PokemonSpecies species in allSpecies)
        {
            if (species == null) continue;

            if (!speciesLookupTable.ContainsKey(species.indexNumber))
            {
                speciesLookupTable.Add(species.indexNumber, species);
            }
            else
            {
                Debug.LogWarning($"PokemonDatabase: Duplicate index entry detected for ID #{species.indexNumber} ({species.speciesName}). Skipping duplicate.");
            }
        }
    }

    /// <summary>
    /// Retrieves a species template directly via its numerical index ID.
    /// </summary>
    /// <param name="indexId">The Index Number ID to search for.</param>
    /// <returns>The matching PokemonSpecies asset, or null if not registered.</returns>
    public PokemonSpecies GetSpeciesById(int indexId)
    {
        if (speciesLookupTable.TryGetValue(indexId, out PokemonSpecies species))
        {
            return species;
        }

        Debug.LogError($"PokemonDatabase: Request failed. Species Index ID #{indexId} is not registered in the database list.");
        return null;
    }
}
