// <copyright file="DialogueBranchingTests.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework.Tests.XUnit;

/// <summary>
/// Tests whether the dialogue runner branches into different dialogue nodes correctly.
/// </summary>
public class DialogueBranchingTests
{
    /// <summary>
    /// Checks whether the dialogue runner can branch from A2 to B1.
    /// </summary>
    [Fact]
    public void Dialogue_BranchIntoB_Correctly()
    {
        IDialogueRunner<string, string, string> runner = GetDialogueRunner();

        Assert.Equal("Welcome, traveler.", runner.Current?.Content);

        // A1 --> A2
        var choicesA1 = runner.GetAvailableChoices();
        Assert.Single(choicesA1);

        bool a2ChooseSuccess = runner.Choose(choicesA1[0]);
        Assert.True(a2ChooseSuccess);
        Assert.Equal("You arrive at the crossroads.", runner.Current?.Content);

        // A2 --> B1
        var choicesA2 = runner.GetAvailableChoices();
        Assert.Equal(2, choicesA2.Count);

        bool b1ChooseSuccess = runner.Choose(choicesA2[0]);
        Assert.True(b1ChooseSuccess);
        Assert.Equal("You encounter a peaceful village.", runner.Current?.Content);

        // B1 --> END
        var choicesB1 = runner.GetAvailableChoices();
        Assert.Single(choicesB1);

        bool endChooseSuccess = runner.Choose(choicesB1[0]);
        Assert.False(endChooseSuccess);
    }

    /// <summary>
    /// Checks whether the dialogue runner can branch from A2 to C1.
    /// </summary>
    [Fact]
    public void Dialogue_BranchIntoC_Correctly()
    {
        IDialogueRunner<string, string, string> runner = GetDialogueRunner();

        Assert.Equal("Welcome, traveler.", runner.Current?.Content);

        // A1 --> A2
        var choicesA1 = runner.GetAvailableChoices();
        Assert.Single(choicesA1);

        bool a2ChooseSuccess = runner.Choose(choicesA1[0]);
        Assert.True(a2ChooseSuccess);
        Assert.Equal("You arrive at the crossroads.", runner.Current?.Content);

        // A2 --> C1
        var choicesA2 = runner.GetAvailableChoices();
        Assert.Equal(2, choicesA2.Count);

        bool c1ChooseSuccess = runner.Choose(choicesA2[1]);
        Assert.True(c1ChooseSuccess);
        Assert.Equal("You walk into a dark forest.", runner.Current?.Content);

        // C1 --> END
        var choicesC1 = runner.GetAvailableChoices();
        Assert.Single(choicesC1);

        bool endChooseSuccess = runner.Choose(choicesC1[0]);
        Assert.False(endChooseSuccess);
    }

    private static IDialogueRunner<string, string, string> GetDialogueRunner()
    {
        return DialogueBuilderFactory.CreateBuilder<string, string, string, string>()
            .AddLinearNode(StringIds.MsgA1, "Welcome, traveler.", StringIds.MsgA2, "Continue")
            .AddMultiChoiceNode(StringIds.MsgA2, "You arrive at the crossroads.")
                .WithChoice(StringIds.MsgB1, "Take the left path.")
                .WithChoice(StringIds.MsgC1, "Take the right path.")
            .EndNode()
            .AddTerminalNode(StringIds.MsgB1, "You encounter a peaceful village.")
            .AddTerminalNode(StringIds.MsgC1, "You walk into a dark forest.")
            .BuildRunner(null, StringIds.MsgA1);
    }

    private static class StringIds
    {
        public const string MsgA1 = "MSG_A_1";
        public const string MsgA2 = "MSG_A_2";
        public const string MsgB1 = "MSG_B_1";
        public const string MsgC1 = "MSG_C_1";
    }
}
