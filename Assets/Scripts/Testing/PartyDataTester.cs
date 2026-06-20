using UnityEngine;

/// <summary>
/// Isolated diagnostic component to verify species databases, move databases, and factory auto-learning logic.
/// </summary>
public class BaseDataTester : MonoBehaviour
{
    [Header("Debug Configuration")]
    [SerializeField] private int testSpeciesId = 1;     // Defaults to Bulbasaur (#001)
    [SerializeField] private int testStartingLevel = 5;  // Simulating Oak's starter gift level

    private void Start()
    {
        // 1. Verify that our database manager singletons are awake and active
        if (PokemonDatabase.Instance == null)
        {
            Debug.LogError("BaseDataTester: Critical Failure. PokemonDatabase instance could not be found on the _GameSystems object.");
            return;
        }

        Debug.Log("--- STARTING ISOLATED BACKEND VALIDATION TEST ---");

        // 2. Fetch the target blueprint asset using our index lookup dictionary system
        PokemonSpecies targetBlueprint = PokemonDatabase.Instance.GetSpeciesById(testSpeciesId);
        if (targetBlueprint == null)
        {
            Debug.LogError($"BaseDataTester: Aborting test. Could not retrieve species blueprint with ID #{testSpeciesId}. Ensure it is dragged into your registry list.");
            return;
        }

        // 3. Command the factory to manufacture our individual tracking profile and roll stats/moves
        PokemonInstance spawnedPokemon = PokemonFactory.CreatePokemon(targetBlueprint, testStartingLevel);
        if (spawnedPokemon == null)
        {
            Debug.LogError("BaseDataTester: Aborting test. Factory engine returned a null instance profile reference.");
            return;
        }

        // 4. Query our runtime profile and output calculations directly to the console panel
        PrintDiagnosticProfile(spawnedPokemon);
    }

    /// <summary>
    /// Parses structural properties and dynamic data layers to print a comprehensive diagnostic readout.
    /// </summary>
    private void PrintDiagnosticProfile(PokemonInstance p)
    {
        Debug.Log($"[FACTORY SUCCESS] Generated Profile: {p.species.speciesName} | Current Level: {p.level}");
        Debug.Log($"[DATA INTEGRITY] Elemental Mapping: Primary: {p.species.primaryType} | Secondary: {p.species.secondaryType}");
        Debug.Log($"[STAT MATHEMATICS] Health Pool: {p.currentHP} / {p.MaxHP} HP");
        Debug.Log($"[STAT MATHEMATICS] Core Traits: Attack: {p.Attack} | Defense: {p.Defense} | Special: {p.Special} | Speed: {p.Speed}");

        Debug.Log("--- EVALUATING AUTO-GENERATED BATTLE MOVESET SLOTS ---");

        BattleMoveInstance[] activeMoves = p.moves;
        for (int i = 0; i < activeMoves.Length; i++)
        {
            if (activeMoves[i] != null && activeMoves[i].baseMove != null)
            {
                Debug.Log($"Slot [{i + 1}]: {activeMoves[i].baseMove.moveName} (Type: {activeMoves[i].baseMove.type} | PP: {activeMoves[i].currentPP}/{activeMoves[i].baseMove.maxPP})");
            }
            else
            {
                Debug.Log($"Slot [{i + 1}]: Empty Space (Null Memory Pointer)");
            }
        }

        Debug.Log("--- DIAGNOSTIC SYSTEM VALIDATION CYCLE COMPLETE ---");
    }
}
