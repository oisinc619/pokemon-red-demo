using UnityEngine;

/// <summary>
/// Static lookup database handling type effectiveness calculations for combat resolution.
/// </summary>
public static class TypeChart
{
    // Fully explicit effectiveness descriptors to maximize structural readability
    private const float SuperEffective = 2.0f;
    private const float Neutral = 1.0f;
    private const float NotVeryEffective = 0.5f;
    private const float NoEffect = 0.0f;

    // 2D matrix layout corresponding strictly to the order of the PokemonType enum rows/columns
    private static readonly float[,] Matrix = new float[,]
    {
        //               Normal              Fire                Water               Electric            Grass               Ice                 Fighting            Poison              Ground              Flying              Psychic             Bug                 Rock                Ghost               Dragon
        /* Normal */  {  Neutral,           Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            NotVeryEffective,   NoEffect,           Neutral           },
        /* Fire   */  {  Neutral,           NotVeryEffective,   NotVeryEffective,   Neutral,            SuperEffective,     SuperEffective,     Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            SuperEffective,     NotVeryEffective,   Neutral,            NotVeryEffective  },
        /* Water  */  {  Neutral,           SuperEffective,     NotVeryEffective,   Neutral,            NotVeryEffective,   Neutral,            Neutral,            Neutral,            SuperEffective,     Neutral,            Neutral,            Neutral,            SuperEffective,     Neutral,            NotVeryEffective  },
        /* Electr */  {  Neutral,           Neutral,            SuperEffective,     NotVeryEffective,   NotVeryEffective,   Neutral,            Neutral,            Neutral,            NoEffect,           SuperEffective,     Neutral,            Neutral,            Neutral,            Neutral,            NotVeryEffective  },
        /* Grass  */  {  Neutral,           NotVeryEffective,   SuperEffective,     Neutral,            NotVeryEffective,   Neutral,            Neutral,            NotVeryEffective,   SuperEffective,     NotVeryEffective,   Neutral,            NotVeryEffective,   SuperEffective,     Neutral,            NotVeryEffective  },
        /* Ice    */  {  Neutral,           Neutral,            NotVeryEffective,   Neutral,            SuperEffective,     NotVeryEffective,   Neutral,            Neutral,            SuperEffective,     SuperEffective,     Neutral,            Neutral,            Neutral,            Neutral,            SuperEffective    },
        /* Fight  */  {  SuperEffective,    Neutral,            Neutral,            Neutral,            Neutral,            SuperEffective,     Neutral,            NotVeryEffective,   Neutral,            NotVeryEffective,   NotVeryEffective,   NotVeryEffective,   SuperEffective,     NoEffect,           Neutral           },
        /* Poison */  {  Neutral,           Neutral,            Neutral,            Neutral,            SuperEffective,     Neutral,            Neutral,            NotVeryEffective,   NotVeryEffective,   Neutral,            Neutral,            SuperEffective,     NotVeryEffective,   NotVeryEffective,   Neutral           },
        /* Ground */  {  Neutral,           SuperEffective,     Neutral,            SuperEffective,     NotVeryEffective,   Neutral,            Neutral,            SuperEffective,     Neutral,            NoEffect,           Neutral,            NotVeryEffective,   SuperEffective,     Neutral,            Neutral           },
        /* Flying */  {  Neutral,           Neutral,            Neutral,            NotVeryEffective,   SuperEffective,     Neutral,            SuperEffective,     Neutral,            Neutral,            Neutral,            Neutral,            SuperEffective,     NotVeryEffective,   Neutral,            Neutral           },
        /* Psych  */  {  Neutral,           Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            SuperEffective,     SuperEffective,     Neutral,            Neutral,            NotVeryEffective,   Neutral,            Neutral,            Neutral,            Neutral           },
        /* Bug    */  {  Neutral,           NotVeryEffective,   Neutral,            Neutral,            SuperEffective,     Neutral,            NotVeryEffective,   SuperEffective,     Neutral,            NotVeryEffective,   SuperEffective,     Neutral,            Neutral,            NotVeryEffective,   Neutral           },
        /* Rock   */  {  Neutral,           SuperEffective,     Neutral,            Neutral,            Neutral,            SuperEffective,     NotVeryEffective,   Neutral,            NotVeryEffective,   SuperEffective,     Neutral,            SuperEffective,     Neutral,            Neutral,            Neutral           },
        /* Ghost  */  {  NoEffect,          Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            SuperEffective,     Neutral,            Neutral,            SuperEffective,     Neutral           },
        /* Dragon */  {  Neutral,           Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            Neutral,            SuperEffective    }
    };

    /// <summary>
    /// Calculates the compound damage multiplier of an attacking move type evaluated against a defender's dual types.
    /// </summary>
    public static float GetMultiplier(PokemonType moveType, PokemonType targetPrimary, PokemonType targetSecondary)
    {
        int row = (int)moveType;
        int col1 = (int)targetPrimary;
        int col2 = (int)targetSecondary;

        float multiplier = Matrix[row, col1];

        if (targetPrimary != targetSecondary)
        {
            multiplier *= Matrix[row, col2];
        }

        return multiplier;
    }
}
