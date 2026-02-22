// <copyright file="DialogueChoice.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An immutable, internal implementation of <see cref="IDialogueChoice{TRegistryKey,TContent}"/>.
/// </summary>
/// <typeparam name="TRegistryKey">
/// The key type used to identify values in the <see cref="IValueRegistry{TKey}"/>.
/// </typeparam>
/// <typeparam name="TContent">
/// The type of displayable data carried by this choice.
/// </typeparam>
internal sealed class DialogueChoice<TRegistryKey, TContent>
    : IDialogueChoice<TRegistryKey, TContent>
    where TRegistryKey : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DialogueChoice{TRegistryKey, TContent}"/> class.
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
        ICondition<TRegistryKey>? condition = null,
        IAction<TRegistryKey>? action = null)
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
    public ICondition<TRegistryKey>? Condition { get; }

    /// <inheritdoc/>
    public IAction<TRegistryKey>? Action { get; }
}
