using UnityEngine;

/// <summary>
/// Isolated diagnostic component that initializes the player profile with a starting party member and inventory assets.
/// </summary>
public class BaseDataTester : MonoBehaviour
{
    [Header("Global System Connections")]
    [SerializeField] private PlayerState playerState;
    [SerializeField] private PokemonDatabase pokemonDatabase;
    [SerializeField] private ItemDatabase itemDatabase;

    [Header("Starter Configuration")]
    [SerializeField] private int starterSpeciesId = 1;      // Defaults to Bulbasaur (#001)
    [SerializeField] private int starterLevel = 5;          // Canonical Oak gift level

    [Header("Starting Inventory Configuration")]
    [SerializeField] private int startingItemId = 1;        // Defaults to Potion (#001)
    [SerializeField] private int startingItemQuantity = 1;  // Replicates PC withdrawal quantity

    private void Start()
    {
        Debug.Log("--- EXECUTE PLAYER INITIALIZATION LOGIC ---");

        // 1. Structural Dependency Checks
        if (playerState == null || pokemonDatabase == null || itemDatabase == null)
        {
            Debug.LogError("BaseDataTester: Aborting initialization. Make sure PlayerState, PokemonDatabase, and ItemDatabase are fully linked in the Inspector.");
            return;
        }

        // 2. Fetch data blueprints from our asset lookup indexes
        PokemonSpecies starterBlueprint = pokemonDatabase.GetSpeciesById(starterSpeciesId);
        ItemData potionBlueprint = itemDatabase.GetItemById(startingItemId);

        // 3. Inject Starter Pokémon into Player's Party Container
        if (starterBlueprint != null)
        {
            // Manufacture the runtime entity data wrapper profiles
            PokemonInstance starterInstance = PokemonFactory.CreatePokemon(starterBlueprint, starterLevel);

            bool partySuccess = playerState.Party.AddPokemon(starterInstance);
            if (partySuccess)
            {
                Debug.Log($"[BOOTSTRAP SUCCESS] Successfully added Level {starterLevel} {starterBlueprint.speciesName} to Player's active combat team!");
            }
            else
            {
                Debug.LogWarning("[BOOTSTRAP FAILURE] Could not add starter to party. Is the team slot full?");
            }
        }

        // 4. Inject Starting Items into Player's Inventory Bag Container
        if (potionBlueprint != null)
        {
            bool inventorySuccess = playerState.Bag.TryAddItem(potionBlueprint, startingItemQuantity);
            if (inventorySuccess)
            {
                Debug.Log($"[BOOTSTRAP SUCCESS] Successfully packed {startingItemQuantity}x {potionBlueprint.itemName} directly into the player's inventory pocket!");
            }
            else
            {
                Debug.LogWarning("[BOOTSTRAP FAILURE] Could not add item to bag. Is the inventory capacity maxed out?");
            }
        }

        Debug.Log("--- PLAYER GAME INITIALIZATION RUNTIME PROCESSED COMPLETELY ---");
    }
}
