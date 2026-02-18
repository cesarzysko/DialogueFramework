// <copyright file="DialogueNode.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Defines a common immutable type for defining dialogue nodes.
/// </summary>
/// <param name="id">The id of the dialogue node.</param>
/// <param name="content">The displayable content of the dialogue node.</param>
/// <param name="choices">The possible dialogue branches of the dialogue node.</param>
/// <typeparam name="TDialogueContent">The type of the content which the dialogue node holds and displays.</typeparam>
/// <typeparam name="TChoiceContent">The type of the content which the dialogue choice holds and displays.</typeparam>
internal sealed class DialogueNode<TDialogueContent, TChoiceContent>(
    NodeId id,
    TDialogueContent content,
    IReadOnlyList<DialogueChoice<TChoiceContent>> choices)
    : IDialogueNode<TDialogueContent, TChoiceContent>
{
    /// <inheritdoc/>
    public NodeId Id { get; } = id;

    /// <inheritdoc/>
    public TDialogueContent Content { get; } = content;

    /// <inheritdoc/>
    public IReadOnlyList<IDialogueChoice<TChoiceContent>> Choices { get; } =
        choices is { Count: > 0 } ? choices : [new DialogueChoice<TChoiceContent>(default)];
}
