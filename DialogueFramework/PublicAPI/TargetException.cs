// <copyright file="TargetException.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Thrown when supplying a graph with nodes where choices have targets not pointing to any node in the graph.
/// </summary>
public class TargetException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TargetException"/> class.
    /// </summary>
    /// <param name="nodes">
    /// The missing node ids.
    /// </param>
    internal TargetException(IReadOnlyList<NodeId> nodes)
        : base("Every node in the graph must target a valid node within the same graph.")
    {
        this.Nodes = nodes;
    }

    /// <summary>
    /// Gets the missing node ids.
    /// </summary>
    internal IReadOnlyList<NodeId> Nodes { get; }
}
