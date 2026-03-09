// <copyright file="ITargetResolver.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Interface used to define custom choice target node resolver.
/// </summary>
internal interface ITargetResolver
{
    /// <summary>
    /// Determines the target node of the choice.
    /// If there is more than one valid choice, the first will be returned.
    /// </summary>
    /// <param name="registry">
    /// The read-only value registry used when determining the target node of the choice.
    /// </param>
    /// <returns>
    /// The target node id.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Throws when there is no valid target node.
    /// </exception>
    public NodeId? Resolve(IReadOnlyValueRegistry? registry);

    /// <summary>
    /// Returns a collection of all possible target nodes, regardless if the conditions are met.
    /// </summary>
    /// <returns>
    /// A collection of all possible target nodes.
    /// </returns>
    public IReadOnlyList<NodeId?> GetAllTargets();
}
