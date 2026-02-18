// <copyright file="DialogueGraph.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Defines a common immutable type for storing dialogue nodes.
/// </summary>
/// <param name="nodes">The dialogue nodes to store.</param>
/// <typeparam name="TDialogueContent">The type of the content which the dialogue node holds and displays.</typeparam>
/// <typeparam name="TChoiceContent">The type of the content which the dialogue choice holds and displays.</typeparam>
internal sealed class DialogueGraph<TDialogueContent, TChoiceContent>(
    IEnumerable<IDialogueNode<TDialogueContent, TChoiceContent>> nodes)
{
    private Dictionary<int, IDialogueNode<TDialogueContent, TChoiceContent>> Nodes { get; } = CreateGraph(nodes);

    /// <summary>
    /// Gets the dialogue node with the given id.
    /// </summary>
    /// <param name="id">The id of the dialogue node which needs to be retrieved.</param>
    /// <returns>The dialogue node with the given id.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the given id does not correspond to any dialogue node in the dialogue graph.
    /// </exception>
    public IDialogueNode<TDialogueContent, TChoiceContent> GetDialogueNode(NodeId id)
    {
        return this.Nodes[id.Value];
    }

    private static Dictionary<int, IDialogueNode<TDialogueContent, TChoiceContent>> CreateGraph(
        IEnumerable<IDialogueNode<TDialogueContent, TChoiceContent>> nodes)
    {
        try
        {
            return nodes.ToDictionary(n => n.Id.Value);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException("Duplicate node IDs found in graph.", nameof(nodes), ex);
        }
    }
}
