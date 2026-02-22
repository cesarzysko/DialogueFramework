// <copyright file="CompositeAction.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An <see cref="IAction{TRegistryKey}"/> that executes a fixed sequence of other actions in order.
/// </summary>
/// <typeparam name="TRegistryKey">
/// The key type used to identify values in the <see cref="IValueRegistry{TKey}"/>.
/// </typeparam>
public class CompositeAction<TRegistryKey> : IAction<TRegistryKey>
    where TRegistryKey : notnull
{
    private readonly IAction<TRegistryKey>[] actions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeAction{TRegistryKey}"/> class.
    /// </summary>
    /// <param name="actions">
    /// The actions to execute in order when this composite action is triggered.
    /// </param>
    public CompositeAction(params IAction<TRegistryKey>[] actions)
    {
        this.actions = actions;
    }

    /// <inheritdoc/>
    public void Execute(IValueRegistry<TRegistryKey>? valueRegistry)
    {
        foreach (var action in this.actions)
        {
            action.Execute(valueRegistry);
        }
    }
}
