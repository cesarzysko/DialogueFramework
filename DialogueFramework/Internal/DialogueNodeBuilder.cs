// <copyright file="DialogueNodeBuilder.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// The default implementation of
/// <see cref="IDialogueNodeBuilder{TRegistryKey,TUserId,TDialogueContent,TChoiceContent}"/> that accumulates nodes and
/// produces a <see cref="DialogueRunner{TRegistryKey,TDialogueContent,TChoiceContent}"/>.
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
internal sealed class DialogueNodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>
    : IDialogueNodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>
    where TUserId : notnull
    where TRegistryKey : notnull
{
    private readonly List<DialogueNode<TRegistryKey, TDialogueContent, TChoiceContent>> nodes = [];

    private readonly NodeIdRegistry<TUserId> registry;

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogueNodeBuilder{TRegistryKey, TUserId, TDialogueContent, TChoiceContent}"/> class.
    /// </summary>
    /// <param name="logger">
    /// An optional logger for internal diagnostics.
    /// </param>
    internal DialogueNodeBuilder(ILogger? logger)
    {
        this.registry = new NodeIdRegistry<TUserId>(logger);
    }

    /// <inheritdoc/>
    public IDialogueRunner<TRegistryKey, TDialogueContent, TChoiceContent> BuildRunner(IValueRegistry<TRegistryKey>? valueRegistry, TUserId startNode)
    {
        if (this.nodes.Count == 0)
        {
            throw new InvalidOperationException("Cannot build dialogue with no nodes.");
        }

        var graph = new DialogueGraph<TRegistryKey, TDialogueContent, TChoiceContent>(this.nodes);
        return new DialogueRunner<TRegistryKey, TDialogueContent, TChoiceContent>(graph, valueRegistry, this.registry.GetInternalId(startNode));
    }

    /// <inheritdoc/>
    public IDialogueNodeChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddMultiChoiceNode(
        TUserId userId,
        TDialogueContent dialogueContent)
    {
        return new DialogueNodeChoiceBuilder(this, userId, dialogueContent);
    }

    /// <inheritdoc/>
    public IDialogueNodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddLinearNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TUserId targetUserId,
        TChoiceContent? choiceContent = default,
        IAction<TRegistryKey>? action = null)
    {
        return this.AddMultiChoiceNode(userId, dialogueContent)
            .WithChoice(targetUserId, choiceContent!, null, action)
            .EndNode();
    }

    /// <inheritdoc/>
    public IDialogueNodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddTerminalNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TChoiceContent? choiceContent = default,
        IAction<TRegistryKey>? action = null)
    {
        return this.AddMultiChoiceNode(userId, dialogueContent)
            .WithEndChoice(choiceContent, null, action)
            .EndNode();
    }

    /// <inheritdoc/>
    public NodeId GetInternalId(TUserId userId)
    {
        return this.registry.GetOrRegister(userId);
    }

    /// <inheritdoc/>
    public void AddDialogueNodeInternal(
        TUserId userId,
        TDialogueContent dialogueContent,
        IReadOnlyList<DialogueChoice<TRegistryKey, TChoiceContent>> choices)
    {
        NodeId id = this.registry.GetOrRegister(userId);
        var dialogueNode = new DialogueNode<TRegistryKey, TDialogueContent, TChoiceContent>(id, dialogueContent, choices);
        this.nodes.Add(dialogueNode);
    }

    /// <summary>
    /// A transient inner builder that accumulates choices for a single dialogue node and commits the node to the
    /// parent <see cref="DialogueNodeBuilder{TRegistryKey,TUserId,TDialogueContent,TChoiceContent}"/> when
    /// <see cref="EndNode"/> is called.
    /// </summary>
    private sealed class DialogueNodeChoiceBuilder : IDialogueNodeChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>
    {
        private readonly IDialogueNodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> parent;
        private readonly TUserId userId;
        private readonly TDialogueContent dialogueContent;
        private readonly List<DialogueChoice<TRegistryKey, TChoiceContent>> choices;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogueNodeChoiceBuilder"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent builder to which the completed node will be committed.
        /// </param>
        /// <param name="userId">
        /// The user-defined identifier for the node being built.
        /// </param>
        /// <param name="dialogueContent">
        /// The displayable content for the node being built.
        /// </param>
        public DialogueNodeChoiceBuilder(
            IDialogueNodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> parent,
            TUserId userId,
            TDialogueContent dialogueContent)
        {
            this.parent = parent;
            this.userId = userId;
            this.dialogueContent = dialogueContent;
            this.choices = [];
        }

        /// <inheritdoc/>
        public IDialogueNodeChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> WithChoice(
            TUserId targetUserId,
            TChoiceContent choiceContent,
            ICondition<TRegistryKey>? condition = null,
            IAction<TRegistryKey>? action = null)
        {
            NodeId targetId = this.parent.GetInternalId(targetUserId);

            var choice = new DialogueChoice<TRegistryKey, TChoiceContent>(choiceContent, targetId, condition, action);
            this.choices.Add(choice);

            return this;
        }

        /// <inheritdoc/>
        public IDialogueNodeChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> WithEndChoice(
            TChoiceContent? choiceContent,
            ICondition<TRegistryKey>? condition = null,
            IAction<TRegistryKey>? action = null)
        {
            var choice = new DialogueChoice<TRegistryKey, TChoiceContent>(choiceContent, null, condition, action);
            this.choices.Add(choice);

            return this;
        }

        /// <inheritdoc/>
        public IDialogueNodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> EndNode()
        {
            this.parent.AddDialogueNodeInternal(this.userId, this.dialogueContent, this.choices);
            return this.parent;
        }
    }
}
