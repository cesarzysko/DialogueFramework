// <copyright file="DialogueNode.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An immutable, internal implementation of <see cref="IDialogueNode{TRegistryKey,TDialogueContent,TChoiceContent}"/>.
/// </summary>
/// <typeparam name="TRegistryKey">
/// The key type used to identify values in the <see cref="IValueRegistry{TKey}"/>.
/// </typeparam>
/// <typeparam name="TDialogueContent">
/// The type of displayable data attached to this node.
/// </typeparam>
/// <typeparam name="TChoiceContent">
/// The type of displayable data attached to each choice in this node.
/// </typeparam>
internal sealed class DialogueNode<TRegistryKey, TDialogueContent, TChoiceContent>
    : IDialogueNode<TRegistryKey, TDialogueContent, TChoiceContent>
    where TRegistryKey : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DialogueNode{TRegistryKey, TDialogueContent, TChoiceContent}"/> class.
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
    internal DialogueNode(
        NodeId id,
        TDialogueContent content,
        IReadOnlyList<DialogueChoice<TRegistryKey, TChoiceContent>> choices)
    {
        this.Id = id;
        this.Content = content;
        this.Choices = choices is { Count: > 0 }
            ? choices
            : throw new ArgumentNullException(nameof(choices), "Each dialogue node must provide at least one choice.");
    }

    /// <inheritdoc/>
    public NodeId Id { get; }

    /// <inheritdoc/>
    public TDialogueContent Content { get; }

    /// <inheritdoc/>
    public IReadOnlyList<IDialogueChoice<TRegistryKey, TChoiceContent>> Choices { get; }
}
