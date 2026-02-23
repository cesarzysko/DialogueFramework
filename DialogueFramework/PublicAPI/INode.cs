// <copyright file="INode.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Represents a single step in a dialogue, consisting of displayable content and one or more selectable choices.
/// </summary>
/// <typeparam name="TDialogueContent">
/// The type of displayable data attached to the node itself.
/// </typeparam>
/// <typeparam name="TChoiceContent">
/// The type of displayable data attached to each of the node's choices.
/// </typeparam>
public interface INode<out TDialogueContent, out TChoiceContent>
{
    /// <summary>
    /// Gets the data to display when the runner arrives at this node.
    /// </summary>
    public TDialogueContent Content { get; }

    /// <summary>
    /// Gets the set of choices the user can take from this node.
    /// </summary>
    internal IReadOnlyList<IChoice<TChoiceContent>> Choices { get; }

    /// <summary>
    /// Gets the internal identifier assigned to this node by the framework.
    /// </summary>
    internal NodeId Id { get; }
}
