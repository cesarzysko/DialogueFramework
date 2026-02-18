// <copyright file="AdventureProvider.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

#pragma warning disable S1192

namespace DialogueFramework.Tests.XUnit;

/// <summary>
/// A static class responsible for building and providing the dialogue runner for the adventure.
/// </summary>
public static class AdventureProvider
{
    private const string AttackLabel = "[ATTACK] ";
    private const string SpellLabel = " [SPELL] ";
    private const string BribeLabel = " [BRIBE] ";
    private const string FleeLabel = "  [FLEE] ";
    private const string DrinkLabel = " [DRINK] ";
    private const string IgnoreLabel = "[IGNORE] ";

    /// <summary>
    /// Builds the runner for the adventure.
    /// </summary>
    /// <param name="state">The starting state of the player.</param>
    /// <param name="startingSceneId">The starting place of the adventure.</param>
    /// <param name="logger">The optional logger used to print out internal messages.</param>
    /// <returns>The runner instance for the adventure.</returns>
    public static IDialogueRunner<string, string> BuildAdventure(PlayerState state, SceneId startingSceneId = SceneId.Intro, ILogger? logger = null)
    {
        return DialogueBuilderFactory.CreateBuilder<SceneId, string, string>(logger)

            // ===  INTRO ===
            .AddLinearNode(
                SceneId.Intro,
                "You stand at the entrance of a dark cavern. Legend speaks of treasure within, but also of great danger.",
                SceneId.CavernEntrance,
                "Enter the cavern.")

            // ===  CAVERN ENTRANCE ===
            .AddMultiChoiceNode(
                SceneId.CavernEntrance,
                "The cavern is cold and damp. Three passages stretch before you: left, center, and right.")
                .WithChoice(SceneId.LeftPassage, "Take the left passage.")
                .WithChoice(SceneId.CenterPassage, "Take the center passage.")
                .WithChoice(SceneId.RightPassage, "Take the right passage.")
                .EndNode()

            // ===  LEFT PASSAGE - GOBLIN ENCOUNTER ===
            .AddMultiChoiceNode(
                SceneId.LeftPassage,
                "You encounter a goblin blocking your path! It snarls menacingly.")
                .WithChoice(
                    SceneId.FightGoblin,
                    $"{AttackLabel}Fight the goblin.",
                    action: new ModifyResourceAction(ResourceType.Health, -15))
                .WithChoice(
                    SceneId.CastFireballOnGoblin,
                    $"{SpellLabel}Cast a fireball on the goblin.",
                    action: new ModifyResourceAction(ResourceType.Mana, -20))
                .WithChoice(
                    SceneId.BribeGoblin,
                    $"{BribeLabel}Offer some gold to the goblin.",
                    action: new ModifyResourceAction(ResourceType.Gold, -999_999))
                .WithChoice(
                    SceneId.RunAway,
                    $"{FleeLabel}Run back to the entrance.")
                .EndNode()

            .AddLinearNode(
                SceneId.FightGoblin,
                "You fight the goblin bravely! You defeat it but take some damage.\n[-15 HEALTH]",
                SceneId.GoblinDefeated,
                "Continue.")
            .AddLinearNode(
                SceneId.CastFireballOnGoblin,
                "You cast a fireball on the goblin. It get's absolutely obliterated.\n[-20 MANA]",
                SceneId.GoblinDefeated,
                "Continue.")
            .AddLinearNode(
                SceneId.BribeGoblin,
                "The goblin grabs your gold and scurries away, laughing.",
                SceneId.GoblinDefeated,
                "Continue.")
            .AddLinearNode(
                SceneId.RunAway,
                "You flee back to the entrance, your heart pounding.",
                SceneId.CavernEntrance,
                "Catch your breath.")

            .AddLinearNode(
                SceneId.GoblinDefeated,
                "With the goblin gone, you look around and find nothing. What a waste of time!",
                SceneId.CavernEntrance,
                "Return to the entrance.")

            // === CENTER PASSAGE - HEALING FOUNTAIN ===
            .AddMultiChoiceNode(
                SceneId.CenterPassage,
                "You discover a mystical fountain glowing with blue light. The water looks rejuvenating.")
                .WithChoice(
                    SceneId.DrinkFromFountain,
                    $"{DrinkLabel}Drink from the fountain.",
                    action: new CompositeAction(
                        new ModifyResourceAction(ResourceType.Health, 30),
                        new ModifyResourceAction(ResourceType.Mana, 20)))
                .WithChoice(
                    SceneId.IgnoreFountain,
                    $"{IgnoreLabel}Leave the fountain alone.")
                .EndNode()

            .AddLinearNode(
                SceneId.DrinkFromFountain,
                "The water is cool and refreshing! Your wounds heal and your mind clears.\n[+30 HEALTH] [+20 MANA]",
                SceneId.CavernEntrance,
                "Return to the entrance.")
            .AddLinearNode(
                SceneId.IgnoreFountain,
                "You wisely avoid the strange fountain. Better safe than sorry.",
                SceneId.CavernEntrance,
                "Return to the entrance.")

            // === RIGHT PASSAGE - DRAGON ENCOUNTER ===
            .AddMultiChoiceNode(
                SceneId.RightPassage,
                "A MASSIVE DRAGON blocks your path! Its eyes glow with ancient intelligence.\nIt speaks: \"Mortal, you may pass... but for a price.\"")
                .WithChoice(
                    SceneId.PhysicalAttackDragon,
                    $"{AttackLabel} Fight the dragon.",
                    action: new CompositeAction(
                        new ModifyResourceAction(ResourceType.Health, -90),
                        new ModifyResourceAction(ResourceType.Gold, 150)))
                .WithChoice(
                    SceneId.MagicAttackDragon,
                    $"{SpellLabel}Cast a powerful spell.",
                    condition: new HasMinimumResourceCondition(ResourceType.Mana, 100),
                    action: new CompositeAction(
                        new ModifyResourceAction(ResourceType.Mana, -100),
                        new ModifyResourceAction(ResourceType.Gold, 200)))
                .WithChoice(
                    SceneId.PayDragon,
                    $"{BribeLabel}Pay the dragon's toll.",
                    condition: new HasMinimumResourceCondition(ResourceType.Gold, 100),
                    action: new ModifyResourceAction(ResourceType.Gold, -100))
                .WithChoice(
                    SceneId.RunFromDragon,
                    $"{FleeLabel}This is madness! Run away!")
                .EndNode()

            .AddLinearNode(
                SceneId.PhysicalAttackDragon,
                "You charge at the dragon! It swats you aside, but you manage to wound it. The dragon retreats, leaving behind some of its hoard.\n[-90 HEALTH] [+150 GOLD]",
                SceneId.TreasureRoom,
                "Explore the treasure room.")
            .AddLinearNode(
                SceneId.MagicAttackDragon,
                "Your spell strikes true! The dragon roars in pain and flies away. You find its entire treasure hoard!\n[-100 MANA] [+200 GOLD]",
                SceneId.TreasureRoom,
                "Explore the treasure room.")
            .AddLinearNode(
                SceneId.PayDragon,
                "The dragon accepts your payment with a satisfied growl: \"You may pass, mortal.\" \nIt gestures to its treasure room.\n[-100 GOLD]",
                SceneId.TreasureRoom,
                "Enter the treasure room.")
            .AddLinearNode(
                SceneId.RunFromDragon,
                "You flee in terror! The dragon's laughter echoes behind you.",
                SceneId.CavernEntrance,
                "Return to the entrance.")

            // === TREASURE ROOM ===
            .AddMultiChoiceNode(
                SceneId.TreasureRoom,
                "You enter the treasure room! Piles of gold and magical artifacts surround you.\nYou can only take one item, which one will you get?")
                .WithChoice(
                    SceneId.TakeChalice,
                    "Take the golden chalice.",
                    action: new ModifyResourceAction(ResourceType.Gold, 100))
                .WithChoice(
                    SceneId.TakePotion,
                    "Take the health potion.",
                    action: new ModifyResourceAction(ResourceType.Health, 50))
                .WithChoice(
                    SceneId.TakeCrystal,
                    "Take the mana crystal",
                    action: new ModifyResourceAction(ResourceType.Mana, 50))
                .EndNode()

            .AddLinearNode(
                SceneId.TakeChalice,
                "You take the golden chalice. Its weight is satisfying in your hands.\n[+100 GOLD]",
                SceneId.Victory,
                "Leave the cavern.")
            .AddLinearNode(
                SceneId.TakePotion,
                "You drink the health potion. Your wounds heal instantly!\n[+50 HEALTH]",
                SceneId.Victory,
                "Leave the cavern.")
            .AddLinearNode(
                SceneId.TakeCrystal,
                "You absorb the mana crystal. Power surges through you!\n[+50 MANA]",
                SceneId.Victory,
                "Leave the cavern.")

            // === VICTORY ===
            .AddTerminalNode(
                SceneId.Victory,
                "You left the cavern alive.")

            .Build(state, startingSceneId);
    }
}
