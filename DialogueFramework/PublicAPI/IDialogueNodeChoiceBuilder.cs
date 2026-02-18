// <copyright file="IDialogueNodeChoiceBuilder.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An interface used to add choices to a dialogue node.
/// </summary>
/// <typeparam name="TUserId">The user-front type to identify dialogue nodes.</typeparam>
/// <typeparam name="TDialogueContent">The displayable content of dialogue nodes.</typeparam>
/// <typeparam name="TChoiceContent">The displayable content of dialogue choices.</typeparam>
public interface IDialogueNodeChoiceBuilder<TUserId, TDialogueContent, TChoiceContent>
    where TUserId : notnull
{
    /// <summary>
    /// Adds a new choice to the dialogue node.
    /// </summary>
    /// <param name="targetUserId">The user-front identifier of the next dialogue node after this choice is selected.</param>
    /// <param name="choiceContent">The optional displayable content of the dialogue choice.</param>
    /// <param name="condition">The optional condition(s) for this choice to be available.</param>
    /// <param name="action">The optional action(s) to perform once this choice is selected.</param>
    /// <returns>Self.</returns>
    public IDialogueNodeChoiceBuilder<TUserId, TDialogueContent, TChoiceContent> WithChoice(
        TUserId targetUserId,
        TChoiceContent choiceContent,
        ICondition? condition = null,
        IAction? action = null);

    /// <summary>
    /// Adds a new end choice (no target node) to the dialogue node.
    /// </summary>
    /// <param name="choiceContent">The optional displayable content of the dialogue choice.</param>
    /// <param name="condition">The optional condition(s) for this choice to be available.</param>
    /// <param name="action">The optional action(s) to perform once this choice is selected.</param>
    /// <returns>Self.</returns>
    public IDialogueNodeChoiceBuilder<TUserId, TDialogueContent, TChoiceContent> WithEndChoice(
        TChoiceContent? choiceContent,
        ICondition? condition = null,
        IAction? action = null);

    /// <summary>
    /// Ends the building process of the current dialogue node.
    /// </summary>
    /// <returns>Parent (Dialogue Node Builder).</returns>
    public IDialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent> EndNode();
}
