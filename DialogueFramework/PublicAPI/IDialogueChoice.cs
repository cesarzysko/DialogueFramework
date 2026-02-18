// <copyright file="IDialogueChoice.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An interface to be used to handle a dialogue choice.
/// </summary>
/// <typeparam name="TContent">The type of the content which the dialogue choice holds and displays.</typeparam>
public interface IDialogueChoice<out TContent>
{
    /// <summary>
    /// Gets the displayable content of the choice.
    /// </summary>
    public TContent? Content { get; }

    /// <summary>
    /// Gets the target id of the next dialogue node, null if none.
    /// </summary>
    internal NodeId? Target { get; }

    /// <summary>
    /// Gets the condition for the dialogue choice to be available.
    /// </summary>
    internal ICondition? Condition { get; }

    /// <summary>
    /// Gets the action performed upon selecting the dialogue choice.
    /// </summary>
    internal IAction? Action { get; }
}
