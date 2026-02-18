// <copyright file="IDialogueRunner.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An interface used to control the flow of the dialogue.
/// </summary>
/// <typeparam name="TDialogueContent">The displayable content of the dialogue node.</typeparam>
/// <typeparam name="TChoiceContent">The displayable content of the dialogue choices.</typeparam>
public interface IDialogueRunner<out TDialogueContent, TChoiceContent>
{
    /// <summary>
    /// Gets the current dialogue node on which the runner is operating.
    /// </summary>
    public IDialogueNode<TDialogueContent, TChoiceContent> Current { get; }

    /// <summary>
    /// Gets a list of all available dialogue choices for the current dialogue node.
    /// </summary>
    /// <returns>A list of available dialogue choices to choose from.</returns>
    public IReadOnlyList<IDialogueChoice<TChoiceContent>> GetAvailableChoices();

    /// <summary>
    /// Gets a list of all dialogue choices for the current dialogue node, including unavailable ones.
    /// </summary>
    /// <returns>A list of all dialogue choices stemming from the current dialogue node.</returns>
    public IReadOnlyList<IDialogueChoice<TChoiceContent>> GetChoices();

    /// <summary>
    /// Performs actions associated with the choice and moves onto the next dialogue node.
    /// </summary>
    /// <param name="choice">The selected dialogue choice to operate on.</param>
    /// <returns>Whether the dialogue can continue or not.</returns>
    public bool Choose(IDialogueChoice<TChoiceContent> choice);

    /// <summary>
    /// Informs whether the dialogue reached a terminal node.
    /// </summary>
    /// <returns>Whether the dialogue reached a terminal node or not.</returns>
    public bool IsCompleted();

    /// <summary>
    /// Restarts the dialogue from the initial start node.
    /// </summary>
    public void Reset();
}
