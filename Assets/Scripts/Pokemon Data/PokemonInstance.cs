using UnityEngine;

/// <summary>
/// Handles individual Pokémon runtime data, stat calculations, and progression tracking.
/// </summary>
[System.Serializable]
public class PokemonInstance
{
    [Header("Data Reference")]
    public PokemonSpecies species;

    [Header("Dynamic State")]
    public int level;
    public int currentExp;
    public int currentHP;

    // Tracks the current active moves known by this specific individual creature (Maximum of 4 slots)
    public BattleMoveInstance[] moves = new BattleMoveInstance[4];

    // Generation I Individual Values (0-15)
    private int ivHP, ivAttack, ivDefense, ivSpeed, ivSpecial;

    // Generation I Stat Experience / Effort Values (0-65535)
    private int evHP, evAttack, evDefense, evSpeed, evSpecial;

    /// <summary>
    /// Initializes a unique instance of a Pokémon with random IVs and base growth parameters.
    /// </summary>
    public PokemonInstance(PokemonSpecies speciesBlueprint, int startingLevel)
    {
        species = speciesBlueprint;
        level = Mathf.Clamp(startingLevel, 1, 100);

        // Roll independent random IV values
        ivAttack = Random.Range(0, 16);
        ivDefense = Random.Range(0, 16);
        ivSpeed = Random.Range(0, 16);
        ivSpecial = Random.Range(0, 16);

        // Derive HP IV from the least significant bits of the other four IVs
        ivHP = ((ivAttack & 1) << 3) | ((ivDefense & 1) << 2) | ((ivSpeed & 1) << 1) | (ivSpecial & 1);

        // Initialize training stats and progression benchmarks
        evHP = evAttack = evDefense = evSpeed = evSpecial = 0;
        currentExp = GetExperienceForLevel(level);
        currentHP = MaxHP;
    }

    /// <summary>
    /// Evaluates experience gains and updates level milestones iteratively.
    /// </summary>
    public void AddExperience(int amount)
    {
        if (level >= 100) return;

        int oldMaxHP = MaxHP;
        currentExp = Mathf.Max(0, currentExp + amount);

        // Continuous threshold verification for multiple level jumps
        bool leveledUp = false;
        while (level < 100 && currentExp >= GetExperienceForLevel(level + 1))
        {
            level++;
            leveledUp = true;
        }

        // Clamp total experience to maximum capacity cap
        int maxLevelExp = GetExperienceForLevel(100);
        if (currentExp > maxLevelExp)
        {
            currentExp = maxLevelExp;
        }

        // Apply health adjustments on level-up
        if (leveledUp)
        {
            int hpGain = MaxHP - oldMaxHP;
            currentHP += hpGain;
        }
    }

    /// <summary>
    /// Returns the exact cumulative experience needed for a specific level based on the species curve.
    /// </summary>
    public int GetExperienceForLevel(int targetLevel)
    {
        float n = targetLevel;

        switch (species.growthRate)
        {
            case GrowthRate.Fast:
                return Mathf.FloorToInt(0.8f * (n * n * n));

            case GrowthRate.MediumFast:
                return Mathf.FloorToInt(n * n * n);

            case GrowthRate.MediumSlow:
                return Mathf.FloorToInt((1.2f * (n * n * n)) - (15f * (n * n)) + (100f * n) - 140f);

            case GrowthRate.Slow:
                return Mathf.FloorToInt(1.25f * (n * n * n));

            default:
                return 0;
        }
    }

    /// <summary>
    /// Calculates the standard stat engine term derived from Stat Experience.
    /// </summary>
    private int CalculateEVTerm(int statExpValue)
    {
        return Mathf.FloorToInt(Mathf.Sqrt(statExpValue) / 4f);
    }

    /// <summary>
    /// Standard non-HP stat generation formula mapping base stats, IVs, and EVs to current level.
    /// </summary>
    private int CalculateStandardStat(int baseValue, int ivValue, int evValue)
    {
        int evTerm = CalculateEVTerm(evValue);
        float topPart = ((baseValue + ivValue) * 2) + evTerm;
        return Mathf.FloorToInt((topPart * level) / 100f) + 5;
    }

    // Dynamic stat exposures recalculated on invocation
    public int MaxHP
    {
        get
        {
            int evTerm = CalculateEVTerm(evHP);
            float topPart = ((species.baseHP + ivHP) * 2) + evTerm;
            return Mathf.FloorToInt((topPart * level) / 100f) + level + 10;
        }
    }

    public int Attack => CalculateStandardStat(species.baseAttack, ivAttack, evAttack);
    public int Defense => CalculateStandardStat(species.baseDefense, ivDefense, evDefense);
    public int Speed => CalculateStandardStat(species.baseSpeed, ivSpeed, evSpeed);
    public int Special => CalculateStandardStat(species.baseSpecial, ivSpecial, evSpecial);
}
