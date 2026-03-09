// <copyright file="CompositeAction.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An <see cref="IAction"/> that executes a fixed sequence of other actions in order.
/// </summary>
public class CompositeAction : IAction
{
    private readonly IAction[] actions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeAction"/> class.
    /// </summary>
    /// <param name="actions">
    /// The actions to execute in order when this composite action is triggered.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Throws if <see cref="actions"/> don't contain any action.
    /// </exception>
    public CompositeAction(params IAction[] actions)
    {
        this.actions = actions is { Length: > 0 }
            ? actions
            : throw new ArgumentException("A composite action must contain at least one action.");
    }

    /// <inheritdoc/>
    public void Execute(IReadWriteValueRegistry? valueRegistry)
    {
        foreach (var action in this.actions)
        {
            action.Execute(valueRegistry);
        }
    }
}
