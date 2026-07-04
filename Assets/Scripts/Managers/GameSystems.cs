using UnityEngine;

/// <summary>
/// Persistent hub managing global references and triggering foundational systems at boot.
/// </summary>
public class GameSystems : MonoBehaviour
{
    [Header("Global Database Assets")]
    [SerializeField] private PokemonDatabase pokemonDatabase;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private BattleMoveDatabase moveDatabase;

    private void Awake()
    {
        // Enforce persistence across scene transitions
        DontDestroyOnLoad(gameObject);

        // Boot up our project file asset lookup tables immediately at startup
        if (pokemonDatabase != null)
        {
            pokemonDatabase.InitializeLookupTable();
        }

        if (itemDatabase != null)
        {
            itemDatabase.InitializeDatabase();
        }

        if (moveDatabase != null)
        {
            moveDatabase.InitializeLookupTable();
        }
    }
}
