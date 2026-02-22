// <copyright file="IDialogueChoice.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Represents a single selectable choice within a dialogue node.
/// </summary>
/// <typeparam name="TRegistryKey">
/// The key type used to identify values in the <see cref="IValueRegistry{TKey}"/>.
/// </typeparam>
/// <typeparam name="TContent">
/// The type of displayable data attached to the choice.
/// </typeparam>
public interface IDialogueChoice<out TRegistryKey, out TContent>
    where TRegistryKey : notnull
{
    /// <summary>
    /// Gets the data to display when presenting this choice to the user.
    /// </summary>
    public TContent? Content { get; }

    /// <summary>
    /// Gets the internal identifier of the node this choice leads to.
    /// </summary>
    internal NodeId? Target { get; }

    /// <summary>
    /// Gets the condition that must be satisfied for this choice to appear as available.
    /// </summary>
    internal ICondition<TRegistryKey>? Condition { get; }

    /// <summary>
    /// Gets the action executed by the runner when this choice is selected.
    /// </summary>
    internal IAction<TRegistryKey>? Action { get; }
}
