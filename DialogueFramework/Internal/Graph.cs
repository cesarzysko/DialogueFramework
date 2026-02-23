// <copyright file="Graph.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An immutable dictionary of dialogue nodes, indexed by their internal <see cref="NodeId"/>, used by
/// <see cref="Runner{TRegistryKey,TDialogueContent,TChoiceContent}"/> to resolve choice targets during
/// traversal.
/// </summary>
/// <typeparam name="TDialogueContent">
/// The type of displayable data carried by each node.
/// </typeparam>
/// <typeparam name="TChoiceContent">
/// The type of displayable data carried by each choice.
/// </typeparam>
internal sealed class Graph<TDialogueContent, TChoiceContent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Graph{TDialogueContent,TChoiceContent}"/>
    /// class.
    /// </summary>
    /// <param name="nodes">
    /// The complete set of nodes that form the dialogue graph. All node IDs must be unique.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if any two nodes in <paramref name="nodes"/> share the same <see cref="NodeId"/>.
    /// </exception>
    internal Graph(IEnumerable<INode<TDialogueContent, TChoiceContent>> nodes)
    {
        this.Nodes = CreateGraph(nodes);
    }

    private Dictionary<int, INode<TDialogueContent, TChoiceContent>> Nodes { get; }

    /// <summary>
    /// Retrieves the node with the given internal identifier.
    /// </summary>
    /// <param name="id">
    /// The internal identifier of the node to retrieve.
    /// </param>
    /// <returns>
    /// The node corresponding to <paramref name="id"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when no node with the given <paramref name="id"/> exists in the graph.
    /// </exception>
    public INode<TDialogueContent, TChoiceContent> GetDialogueNode(NodeId id)
    {
        return this.Nodes[id.Value];
    }

    /// <summary>
    /// Converts the flat sequence of nodes into a dictionary keyed by node ID.
    /// </summary>
    /// <param name="nodes">
    /// The nodes to index.
    /// </param>
    /// <returns>
    /// A dictionary from raw integer ID to node.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when two or more nodes share the same <see cref="NodeId"/>.
    /// </exception>
    private static Dictionary<int, INode<TDialogueContent, TChoiceContent>> CreateGraph(
        IEnumerable<INode<TDialogueContent, TChoiceContent>> nodes)
    {
        Dictionary<int, INode<TDialogueContent, TChoiceContent>> graph;

        try
        {
            graph = nodes.ToDictionary(n => n.Id.Value);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException("Duplicate node IDs found in graph.", nameof(nodes), ex);
        }

        NodeId[] missingTargets = graph.Values
            .SelectMany(node => node.Choices)
            .Select(choice => choice.Target)
            .Where(t => t != null && !graph.ContainsKey(t.Value.Value))
            .Select(t => t!.Value)
            .ToArray();

        if (missingTargets.Length != 0)
        {
            throw new TargetException(missingTargets);
        }

        return graph;
    }
}
