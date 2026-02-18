// <copyright file="SceneId.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework.Tests.XUnit;

/// <summary>
/// Enum representing all scenes in the adventure.
/// </summary>
public enum SceneId
{
    Intro,

    CavernEntrance,

    LeftPassage,
    FightGoblin,
    CastFireballOnGoblin,
    BribeGoblin,
    GoblinDefeated,
    RunAway,

    CenterPassage,
    DrinkFromFountain,
    IgnoreFountain,

    RightPassage,
    MagicAttackDragon,
    PhysicalAttackDragon,
    PayDragon,
    RunFromDragon,

    TreasureRoom,
    TakeChalice,
    TakePotion,
    TakeCrystal,

    Victory,
}
