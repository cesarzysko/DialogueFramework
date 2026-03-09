// <copyright file="IChoice.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Represents a single selectable choice within a dialogue node.
/// </summary>
/// <typeparam name="TContent">
/// The type of displayable data attached to the choice.
/// </typeparam>
public interface IChoice<out TContent>
{
    /// <summary>
    /// Gets the data to display when presenting this choice to the user.
    /// </summary>
    public TContent? Content { get; }

    /// <summary>
    /// Gets the condition that must be satisfied for this choice to appear as available.
    /// </summary>
    internal ICondition? Condition { get; }

    /// <summary>
    /// Gets the action executed by the runner when this choice is selected.
    /// </summary>
    internal IAction? Action { get; }

    /// <summary>
    /// Gets the internal identifier of the node this choice leads to.
    /// </summary>
    /// <param name="valueRegistry">
    /// The read-only value registry used to determine the returned target node.
    /// </param>
    /// <returns>
    /// The id of the target node this choice leads to.
    /// </returns>
    internal NodeId? GetTarget(IReadOnlyValueRegistry? valueRegistry);

    /// <summary>
    /// Returns all possible targets, regardless of whether their conditions are met.
    /// </summary>
    /// <returns>
    /// List of all possible targets for this choice.
    /// </returns>
    internal IReadOnlyList<NodeId?> GetAllTargets();
}
