// <copyright file="TextAdventureTests.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework.Tests.XUnit;

using Xunit.Abstractions;

/// <summary>
/// Tests whether the sample text adventure behaves correctly.
/// </summary>
public class TextAdventureTests
{
    private readonly ITestOutputHelper testOutputHelper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextAdventureTests"/> class.
    /// </summary>
    /// <param name="testOutputHelper">Used to output test messages.</param>
    public TextAdventureTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    /// <summary>
    /// Tests whether the sample text adventure can be successfully completed when taking the given route.
    /// </summary>
    /// <param name="choices">The choices to be made in order to complete the adventure.</param>
    /// <param name="expectedResult">Whether the adventure is completed or not after making the given choices.</param>
    [Theory]
    [InlineData(new[] { 0, 0, 0, 0, 0, 1, 0, 0, 2, 1, 0, 0, 0, 0 }, true)]
    public void TextAdventure_TakePath_CanComplete(int[] choices, bool expectedResult)
    {
        this.testOutputHelper.WriteLine("\n////////////////////////////////////////");

        var adventure = AdventureProvider.BuildAdventure(logger: new ConsoleLogger(this.testOutputHelper));

        int i = -1;
        int n = choices.Length;
        while (++i < n)
        {
            this.testOutputHelper.WriteLine(adventure.Current?.Content);

            var allChoices = adventure.GetChoices();
            var availableChoices = adventure.GetAvailableChoices();

            int chN = allChoices.Count;
            for (int j = 0; j < chN; ++j)
            {
                var choice = allChoices[j];
                var choiceContent = string.IsNullOrWhiteSpace(choice.Content)
                    ? "END OF STORY"
                    : choice.Content;
                this.testOutputHelper.WriteLine(
                    availableChoices.Contains(choice)
                        ? $"{j + 1}. {choiceContent}"
                        : $"{j + 1}. {choiceContent} [NOT AVAILABLE]");
            }

            var selectedChoice = allChoices[choices[i]];
            var selectedChoiceContent = string.IsNullOrWhiteSpace(selectedChoice.Content)
                ? "END OF STORY"
                : selectedChoice.Content;

            this.testOutputHelper.WriteLine($"\n > {selectedChoiceContent}\n");
            this.testOutputHelper.WriteLine("////////////////////////////////////////");

            bool choiceAccepted = adventure.Choose(selectedChoice);
            if (!choiceAccepted)
            {
                break;
            }
        }

        Assert.Equal(expectedResult, adventure.ReachedTerminalNode());
    }
}
