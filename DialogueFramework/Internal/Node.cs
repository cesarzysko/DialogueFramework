// <copyright file="Node.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An immutable, internal implementation of <see cref="INode{TDialogueContent,TChoiceContent}"/>.
/// </summary>
/// <typeparam name="TDialogueContent">
/// The type of displayable data attached to this node.
/// </typeparam>
/// <typeparam name="TChoiceContent">
/// The type of displayable data attached to each choice in this node.
/// </typeparam>
internal sealed class Node<TDialogueContent, TChoiceContent>
    : INode<TDialogueContent, TChoiceContent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Node{TDialogueContent,TChoiceContent}"/> class.
    /// </summary>
    /// <param name="id">
    /// The internal identifier assigned to this node by <see cref="NodeIdRegistry{TUserId}"/>.
    /// </param>
    /// <param name="content">
    /// The data to display when the runner arrives at this node.
    /// </param>
    /// <param name="choices">
    /// The ordered list of choices the user may take from this node.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Throws when there are no choices provided.
    /// </exception>
    internal Node(
        NodeId id,
        TDialogueContent content,
        IReadOnlyList<Choice<TChoiceContent>> choices)
    {
        this.Id = id;
        this.Content = content;
        this.Choices = choices is { Count: > 0 }
            ? choices
            : throw new ArgumentException("Each dialogue node must provide at least one choice.", nameof(choices));
    }

    /// <inheritdoc/>
    public NodeId Id { get; }

    /// <inheritdoc/>
    public TDialogueContent Content { get; }

    /// <inheritdoc/>
    public IReadOnlyList<IChoice<TChoiceContent>> Choices { get; }
}
