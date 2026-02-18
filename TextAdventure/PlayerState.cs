// <copyright file="PlayerState.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework.Tests.XUnit;

/// <summary>
/// Game state tracking player resources.
/// </summary>
public class PlayerState : IVariableStore
{
    private int health;
    private int mana;
    private int gold;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerState"/> class.
    /// </summary>
    /// <param name="health">The health points of the player, required to continue the adventure.</param>
    /// <param name="mana">The mana points of the player, required to cast spells.</param>
    /// <param name="gold">The gold points of the player, required for bribes, buying items, etc.</param>
    public PlayerState(int health, int mana, int gold)
    {
        this.health = health;
        this.mana = mana;
        this.gold = gold;
    }

    /// <inheritdoc/>
    public bool TryGet<TKey, TValue>(TKey key, out TValue value)
    {
        value = default!;

        if (key is not ResourceType resourceType)
        {
            return false;
        }

        object? result = resourceType switch
        {
            ResourceType.Health => this.health,
            ResourceType.Mana => this.mana,
            ResourceType.Gold => this.gold,
            _ => null,
        };

        if (result is not TValue typedValue)
        {
            return false;
        }

        value = typedValue;
        return true;
    }

    /// <inheritdoc/>
    public bool TrySet<TKey, TValue>(TKey key, TValue value)
    {
        if (key is not ResourceType resourceType)
        {
            return false;
        }

        return resourceType switch
        {
            ResourceType.Health => TrySetIntValue(ref this.health, value, 0),
            ResourceType.Mana => TrySetIntValue(ref this.mana, value, 0),
            ResourceType.Gold => TrySetIntValue(ref this.gold, value, 0),
            _ => false,
        };
    }

    /// <summary>
    /// Constructs a message containing player stats and total score.
    /// </summary>
    /// <returns>String containing player stats and total score.</returns>
    public string GetPlayerStateMessage()
    {
        return $"[HEALTH]: {this.health}\n" +
               $"  [MANA]: {this.mana}\n" +
               $"  [GOLD]: {this.gold}\n" +
               $"> TOTAL SCORE: {(2 * this.health) + this.mana + (this.gold / 2)}";
    }

    private static bool TrySetIntValue<TValue>(ref int value, TValue newValue, int minValue)
    {
        if (newValue is not int typedValue)
        {
            return false;
        }

        value = Math.Max(minValue, typedValue);
        return true;
    }
}
