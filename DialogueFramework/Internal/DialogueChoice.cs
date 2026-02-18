// <copyright file="DialogueChoice.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Defines a common immutable type for defining dialogue choices.
/// </summary>
/// <param name="content">The displayable content of the choice.</param>
/// <param name="target">The target id of the next dialogue node, null if none.</param>
/// <param name="condition">The condition for the dialogue choice to be available.</param>
/// <param name="action">The action performed upon selecting the dialogue choice.</param>
/// <typeparam name="TContent">The type of the content which the dialogue choice holds and displays.</typeparam>
internal sealed class DialogueChoice<TContent>(
    TContent? content,
    NodeId? target = null,
    ICondition? condition = null,
    IAction? action = null)
    : IDialogueChoice<TContent>
{
    /// <inheritdoc/>
    TContent? IDialogueChoice<TContent>.Content { get; } = content;

    /// <inheritdoc/>
    NodeId? IDialogueChoice<TContent>.Target { get; } = target;

    /// <inheritdoc/>
    ICondition? IDialogueChoice<TContent>.Condition { get; } = condition;

    /// <inheritdoc/>
    IAction? IDialogueChoice<TContent>.Action { get; } = action;
}
