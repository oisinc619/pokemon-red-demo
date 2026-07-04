using UnityEngine;

/// <summary>
/// Central manager tracking persistent player statistics, inventory, and active party records.
/// Attached to the global GameSystems hub.
/// </summary>
[RequireComponent(typeof(PokemonParty))]
[RequireComponent(typeof(InventoryBag))]
public class PlayerState : MonoBehaviour
{
    // Public read-only property hooks exposing the sub-systems
    public PokemonParty Party { get; private set; }
    public InventoryBag Bag { get; private set; }

    [Header("Trainer Identity Profile")]
    [SerializeField] private string playerName = "PLAYER";
    [SerializeField] private int money = 3000;

    /// <summary>
    /// Read-only access to the player's current financial wallet balance.
    /// </summary>
    public int Money => money;

    private void Awake()
    {
        // Cache the required component modules on system initialization
        Party = GetComponent<PokemonParty>();
        Bag = GetComponent<InventoryBag>();
    }

    /// <summary>
    /// Dynamically overrides the fallback character name string.
    /// </summary>
    /// <param name="newName">The validated string input chosen from the naming screen interface.</param>
    public void SetPlayerName(string newName)
    {
        if (!string.IsNullOrEmpty(newName))
        {
            playerName = newName;
        }
    }

    /// <summary>
    /// Modifies the player's current wallet balance safely, preventing negative balances.
    /// </summary>
    /// <param name="amount">The cash total to add (positive) or deduct (negative).</param>
    /// <returns>True if the transaction succeeded; false if funds are insufficient.</returns>
    public bool TryModifyMoney(int amount)
    {
        if (money + amount < 0)
        {
            Debug.LogWarning($"PlayerState: Transaction failed. Insufficient funds to deduct {Mathf.Abs(amount)}.");
            return false;
        }

        money += amount;
        return true;
    }
}
