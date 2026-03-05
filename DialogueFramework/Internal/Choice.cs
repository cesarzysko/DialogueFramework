// <copyright file="Choice.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An immutable, internal implementation of <see cref="IChoice{TContent}"/>.
/// </summary>
/// <typeparam name="TContent">
/// The type of displayable data carried by this choice.
/// </typeparam>
internal sealed class Choice<TContent>
    : IChoice<TContent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Choice{TContent}"/> class.
    /// </summary>
    /// <param name="content">
    /// The data to display when presenting this choice to the user.
    /// </param>
    /// <param name="targetResolver">
    /// Function to use to get the internal identifier of the node this choice leads to.
    /// </param>
    /// <param name="condition">
    /// The predicate evaluated to determine whether this choice is available.
    /// </param>
    /// <param name="action">
    /// The effect executed when this choice is selected.
    /// </param>
    internal Choice(
        TContent? content,
        Func<IReadOnlyValueRegistry?, NodeId?>? targetResolver = null,
        ICondition? condition = null,
        IAction? action = null)
    {
        this.Content = content;
        this.TargetResolver = targetResolver;
        this.Condition = condition;
        this.Action = action;
    }

    /// <inheritdoc/>
    public TContent? Content { get; }

    /// <inheritdoc/>
    public ICondition? Condition { get; }

    /// <inheritdoc/>
    public IAction? Action { get; }

    private Func<IReadOnlyValueRegistry?, NodeId?>? TargetResolver { get; }

    /// <inheritdoc/>
    public NodeId? GetTarget(IReadOnlyValueRegistry? valueRegistry)
    {
        return this.TargetResolver?.Invoke(valueRegistry) ?? null;
    }
}
