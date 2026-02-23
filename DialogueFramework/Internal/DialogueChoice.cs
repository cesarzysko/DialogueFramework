// <copyright file="DialogueChoice.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An immutable, internal implementation of <see cref="IDialogueChoice{TRegistryKey,TContent}"/>.
/// </summary>
/// <typeparam name="TContent">
/// The type of displayable data carried by this choice.
/// </typeparam>
internal sealed class DialogueChoice<TContent>
    : IDialogueChoice<TContent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DialogueChoice{TContent}"/> class.
    /// </summary>
    /// <param name="content">
    /// The data to display when presenting this choice to the user.
    /// </param>
    /// <param name="target">
    /// The internal identifier of the node this choice leads to.
    /// </param>
    /// <param name="condition">
    /// The predicate evaluated to determine whether this choice is available.
    /// </param>
    /// <param name="action">
    /// The effect executed when this choice is selected.
    /// </param>
    internal DialogueChoice(
        TContent? content,
        NodeId? target = null,
        ICondition? condition = null,
        IAction? action = null)
    {
        this.Content = content;
        this.Target = target;
        this.Condition = condition;
        this.Action = action;
    }

    /// <inheritdoc/>
    public TContent? Content { get; }

    /// <inheritdoc/>
    public NodeId? Target { get; }

    /// <inheritdoc/>
    public ICondition? Condition { get; }

    /// <inheritdoc/>
    public IAction? Action { get; }
}
