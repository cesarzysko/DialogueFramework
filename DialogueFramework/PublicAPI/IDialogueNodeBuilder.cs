// <copyright file="IDialogueNodeBuilder.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An interface used to create dialogues.
/// </summary>
/// <typeparam name="TUserId">The user-front type used for dialogues identification.</typeparam>
/// <typeparam name="TDialogueContent">The displayable content of dialogues.</typeparam>
/// <typeparam name="TChoiceContent">The displayable content of dialogue choices.</typeparam>
public interface IDialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent>
    where TUserId : notnull
{
    /// <summary>
    /// Creates a dialogue node and allows to specify each dialogue choice.
    /// </summary>
    /// <param name="userId">The user-front dialogue identifier.</param>
    /// <param name="dialogueContent">The displayable content of the created dialogue node.</param>
    /// <returns>A builder used to define dialogue node choices.</returns>
    public IDialogueNodeChoiceBuilder<TUserId, TDialogueContent, TChoiceContent> AddMultiChoiceNode(
        TUserId userId,
        TDialogueContent dialogueContent);

    /// <summary>
    /// Creates a dialogue node with a single choice leading to another dialogue node.
    /// </summary>
    /// <param name="userId">The user-front identification of the dialogue node.</param>
    /// <param name="dialogueContent">The displayable content of the dialogue node.</param>
    /// <param name="targetUserId">The user-front identification of the dialogue node following this dialogue node.</param>
    /// <param name="choiceContent">The optional displayable content of the dialogue choice.</param>
    /// <param name="action">The optional action triggered once selecting the dialogue choice.</param>
    /// <returns>Self.</returns>
    public IDialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent> AddLinearNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TUserId targetUserId,
        TChoiceContent? choiceContent = default,
        IAction? action = null);

    /// <summary>
    /// Creates a dialogue node with a single choice leading to the dialogue end.
    /// </summary>
    /// <param name="userId">The user-front identification of the dialogue node.</param>
    /// <param name="dialogueContent">The displayable content of the dialogue node.</param>
    /// <param name="choiceContent">The optional displayable content of the dialogue choice.</param>
    /// <param name="action">The optional action triggered once selecting the dialogue choice.</param>
    /// <returns>Self.</returns>
    public IDialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent> AddTerminalNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TChoiceContent? choiceContent = default,
        IAction? action = null);

    /// <summary>
    /// Creates a dialogue runner using previously created dialogue nodes.
    /// </summary>
    /// <param name="variableStore">The source of values used in choice conditions and actions.</param>
    /// <param name="startNode">The user-front dialogue node identifier, from which the dialogue should start running.</param>
    /// <returns>The created dialogue runner.</returns>
    public IDialogueRunner<TDialogueContent, TChoiceContent> Build(
        IVariableStore? variableStore,
        TUserId startNode);

    /// <summary>
    /// Returns the internal id of the dialogue node corresponding to the given user-front identifier.
    /// </summary>
    /// <param name="userId">The user-front identifier corresponding to the returned internal id.</param>
    /// <returns>The internal id.</returns>
    internal NodeId GetInternalId(TUserId userId);

    /// <summary>
    /// Adds the node to the nodes list once all choices are decided.
    /// </summary>
    /// <param name="userId">The user-front identifier of the dialogue node.</param>
    /// <param name="dialogueContent">The displayable content of the dialogue node.</param>
    /// <param name="choices">The choices of the dialogue node.</param>
    internal void AddDialogueNodeInternal(
        TUserId userId,
        TDialogueContent dialogueContent,
        IReadOnlyList<DialogueChoice<TChoiceContent>> choices);
}
