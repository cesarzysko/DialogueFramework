// <copyright file="IDialogueNodeBuilder.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// A builder for constructing a dialogue graph node by node, culminating in an
/// <see cref="IDialogueRunner{TDialogueContent,TChoiceContent}"/> that traverses the finished graph.
/// </summary>
/// <typeparam name="TRegistryKey">
/// The key type used to identify values in the <see cref="IValueRegistry{TKey}"/>.
/// </typeparam>
/// <typeparam name="TUserId">
/// The user-defined type used to name and reference dialogue nodes.
/// </typeparam>
/// <typeparam name="TDialogueContent">
/// The type of displayable data carried by each dialogue node.
/// </typeparam>
/// <typeparam name="TChoiceContent">
/// The type of displayable data carried by each choice.
/// </typeparam>
public interface IDialogueNodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>
    where TUserId : notnull
    where TRegistryKey : notnull
{
    /// <summary>
    /// Begins defining a branching node with multiple selectable choices.
    /// </summary>
    /// <param name="userId">
    /// A unique user-defined identifier for this node.
    /// </param>
    /// <param name="dialogueContent">
    /// The data to display when the runner arrives at this node.
    /// </param>
    /// <returns>
    /// A <see cref="IDialogueNodeChoiceBuilder{TRegistryKey,TUserId,TDialogueContent,TChoiceContent}"/> for adding
    /// choices to this node.
    /// </returns>
    public IDialogueNodeChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddMultiChoiceNode(
        TUserId userId,
        TDialogueContent dialogueContent);

    /// <summary>
    /// Creates a dialogue node with a single choice leading to another dialogue node.
    /// </summary>
    /// <param name="userId">
    /// A unique user-defined identifier for this node.
    /// </param>
    /// <param name="dialogueContent">
    /// The data to display when the runner arrives at this node.
    /// </param>
    /// <param name="targetUserId">
    /// The identifier of the node to advance to when the choice is selected.
    /// </param>
    /// <param name="choiceContent">
    /// Optional data to display for the single choice.
    /// </param>
    /// <param name="action">
    /// An optional effect to execute when the choice is selected.
    /// </param>
    /// <returns>
    /// This builder instance, enabling method chaining.
    /// </returns>
    public IDialogueNodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddLinearNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TUserId targetUserId,
        TChoiceContent? choiceContent = default,
        IAction? action = null);

    /// <summary>
    /// Adds a node with a single choice that ends the dialogue rather than advancing to another node.
    /// </summary>
    /// <param name="userId">
    /// A unique user-defined identifier for this node.
    /// </param>
    /// <param name="dialogueContent">
    /// The data to display when the runner arrives at this node.
    /// </param>
    /// <param name="choiceContent">
    /// Optional data to display for the terminal choice.
    /// </param>
    /// <param name="action">
    /// An optional effect to execute when the choice is selected.
    /// </param>
    /// <returns>
    /// This builder instance, enabling method chaining.
    /// </returns>
    public IDialogueNodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddTerminalNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TChoiceContent? choiceContent = default,
        IAction? action = null);

    /// <summary>
    /// Constructs the dialogue graph from all previously added nodes and returns a runner positioned at the specified
    /// start node, ready for traversal.
    /// </summary>
    /// <param name="valueRegistry">
    /// The registry made available to <see cref="ICondition"/> and <see cref="IAction"/>
    /// implementations at runtime.
    /// </param>
    /// <param name="startNode">
    /// The USER-defined identifier of the node at which the runner should begin.
    /// </param>
    /// <returns>
    /// An <see cref="IDialogueRunner{TDialogueContent,TChoiceContent}"/> ready to traverse the
    /// constructed graph.
    /// </returns>
    public IDialogueRunner<TDialogueContent, TChoiceContent> BuildRunner(
        IValueRegistry<TRegistryKey>? valueRegistry,
        TUserId startNode);

    /// <summary>
    /// Returns the internal <see cref="NodeId"/> assigned to the given user-defined identifier, registering a new one
    /// if the identifier has not been seen before.
    /// </summary>
    /// <param name="userId">
    /// The user-defined identifier to look up or register.
    /// </param>
    /// <returns>
    /// The corresponding internal <see cref="NodeId"/>.
    /// </returns>
    internal NodeId GetInternalId(TUserId userId);

    /// <summary>
    /// Registers a fully constructed node into the builder's node list.
    /// </summary>
    /// <param name="userId">
    /// The user-defined identifier of the node being added.
    /// </param>
    /// <param name="dialogueContent">
    /// The displayable content of the node.
    /// </param>
    /// <param name="choices">
    /// The ordered list of choices attached to the node.
    /// </param>
    internal void AddDialogueNodeInternal(
        TUserId userId,
        TDialogueContent dialogueContent,
        IReadOnlyList<DialogueChoice<TChoiceContent>> choices);
}
