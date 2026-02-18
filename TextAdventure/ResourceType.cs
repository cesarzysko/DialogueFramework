// <copyright file="ResourceType.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework.Tests.XUnit;

/// <summary>
/// Resource types used in the game.
/// </summary>
public enum ResourceType
{
    /// <summary>
    /// The health points of the player, required to continue the adventure.
    /// </summary>
    Health,

    /// <summary>
    /// The mana points of the player, required to cast spells.
    /// </summary>
    Mana,

    /// <summary>
    /// The gold amount of the player, required for bribes, buying items, etc.
    /// </summary>
    Gold,
}
