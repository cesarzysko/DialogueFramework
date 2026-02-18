// <copyright file="IDialogueNode.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An interface to be used to handle a dialogue node.
/// </summary>
/// <typeparam name="TDialogueContent">The type of the content which the dialogue node holds and displays.</typeparam>
/// <typeparam name="TChoiceContent">The type of the content which the dialogue choice holds and displays.</typeparam>
public interface IDialogueNode<out TDialogueContent, out TChoiceContent>
{
    /// <summary>
    /// Gets the displayable content of the dialogue node.
    /// </summary>
    public TDialogueContent Content { get; }

    /// <summary>
    /// Gets the possible dialogue branches of the dialogue node.
    /// </summary>
    internal IReadOnlyList<IDialogueChoice<TChoiceContent>> Choices { get; }

    /// <summary>
    /// Gets the id of the dialogue node.
    /// </summary>
    internal NodeId Id { get; }
}
