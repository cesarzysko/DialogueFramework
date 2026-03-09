// <copyright file="TargetResolverBuilderBase.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Used as a base for common tasks performed by target resolver builders.
/// </summary>
/// <typeparam name="TUserId">
/// The user-defined type used for node identification.
/// </typeparam>
public abstract class TargetResolverBuilderBase<TUserId>
    where TUserId : notnull
{
    private readonly List<TargetEntry> entries = [];

    /// <summary>
    /// Adds a new target with a conditions which must be met.
    /// </summary>
    /// <param name="condition">
    /// The condition which must be met for this target to be valid.
    /// </param>
    /// <param name="targetUserId">
    /// The user-defined identification of the target node.
    /// </param>
    private protected void AddTarget(ICondition condition, TUserId targetUserId)
    {
        NodeId nodeId = this.ToNodeId(targetUserId);
        var entry = new TargetEntry(condition, nodeId);
        this.entries.Add(entry);
    }

    /// <summary>
    /// Adds a new terminal target with a condition which must be met.
    /// </summary>
    /// <param name="condition">
    /// The condition which must be met for this target to be valid.
    /// </param>
    private protected void AddTerminal(ICondition condition)
    {
        var entry = new TargetEntry(condition, null);
        this.entries.Add(entry);
    }

    /// <summary>
    /// Adds a new fallback target without any condition to be met.
    /// </summary>
    /// <param name="targetUserId">
    /// The user-defined identification of the target node.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Throws when an unconditional fallback already exists.
    /// </exception>
    private protected void AddFallback(TUserId targetUserId)
    {
        if (this.entries.Any(e => e.Condition == null))
        {
            throw new InvalidOperationException("An unconditional fallback already exists.");
        }

        NodeId nodeId = this.ToNodeId(targetUserId);
        var entry = new TargetEntry(null, nodeId);
        this.entries.Add(entry);
    }

    /// <summary>
    /// Adds a new terminal fallback target without any condition to be met.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Throws when an unconditional fallback already exists.
    /// </exception>
    private protected void AddTerminalFallback()
    {
        if (this.entries.Any(e => e.Condition == null))
        {
            throw new InvalidOperationException("An unconditional fallback already exists.");
        }

        var entry = new TargetEntry(null, null);
        this.entries.Add(entry);
    }

    /// <summary>
    /// Returns the internal node id corresponding to the given user-defined id.
    /// </summary>
    /// <param name="userId">
    /// The user-defined identification of the target node.
    /// </param>
    /// <returns>
    /// The node id used to identify a node internally.
    /// </returns>
    private protected abstract NodeId ToNodeId(TUserId userId);

    /// <summary>
    /// Returns all target entries.
    /// </summary>
    /// <returns>
    /// All target entries.
    /// </returns>
    private protected IReadOnlyList<TargetEntry> GetEntries()
    {
        return this.entries;
    }
}
