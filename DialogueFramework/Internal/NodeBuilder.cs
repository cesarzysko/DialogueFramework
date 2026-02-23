// <copyright file="NodeBuilder.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// The default implementation of
/// <see cref="INodeBuilder{TRegistryKey,TUserId,TDialogueContent,TChoiceContent}"/> that accumulates nodes and
/// produces a <see cref="Runner{TRegistryKey,TDialogueContent,TChoiceContent}"/>.
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
internal sealed class NodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>
    : INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>
    where TUserId : notnull
    where TRegistryKey : notnull
{
    private readonly List<Node<TDialogueContent, TChoiceContent>> nodes = [];

    private readonly NodeIdRegistry<TUserId> registry;

    /// <summary>
    /// Initializes a new instance of the <see cref="NodeBuilder{TRegistryKey,TUserId,TDialogueContent,TChoiceContent}"/> class.
    /// </summary>
    /// <param name="logger">
    /// An optional logger for internal diagnostics.
    /// </param>
    internal NodeBuilder(ILogger? logger)
    {
        this.registry = new NodeIdRegistry<TUserId>(logger);
    }

    /// <inheritdoc/>
    public INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddLinearNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TUserId targetUserId,
        TChoiceContent? choiceContent = default,
        IAction? action = null)
    {
        var choice = new Choice<TChoiceContent>(choiceContent, this.GetInternalId(targetUserId));
        this.AddDialogueNodeInternal(userId, dialogueContent, [choice]);
        return this;
    }

    /// <inheritdoc/>
    public INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddTerminalNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TChoiceContent? choiceContent = default,
        IAction? action = null)
    {
        var choice = new Choice<TChoiceContent>(choiceContent);
        this.AddDialogueNodeInternal(userId, dialogueContent, [choice]);
        return this;
    }

    /// <inheritdoc/>
    public IChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddMultiChoiceNode(
        TUserId userId,
        TDialogueContent dialogueContent)
    {
        return new ChoiceBuilder(this, userId, dialogueContent);
    }

    /// <inheritdoc/>
    public IRunner<TDialogueContent, TChoiceContent> BuildRunner(
        IValueRegistry<TRegistryKey>? valueRegistry,
        TUserId startNode)
    {
        if (this.nodes.Count == 0)
        {
            throw new InvalidOperationException("Cannot build dialogue with no nodes.");
        }

        Graph<TDialogueContent, TChoiceContent> graph;
        try
        {
            graph = new Graph<TDialogueContent, TChoiceContent>(this.nodes);
        }
        catch (TargetException nodeTargetException)
        {
            string msg = "The following node targets were not found within the graph: ";
            msg += string.Join(", ", nodeTargetException.Nodes.Select(n => this.registry.GetUserId(n)));
            throw new InvalidOperationException(msg, nodeTargetException);
        }

        NodeId startNodeId = this.GetInternalId(startNode);
        return new Runner<TRegistryKey, TDialogueContent, TChoiceContent>(graph, valueRegistry, startNodeId);
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
        IReadOnlyList<Choice<TChoiceContent>> choices)
    {
        NodeId id = this.registry.GetOrRegister(userId);
        var dialogueNode = new Node<TDialogueContent, TChoiceContent>(id, dialogueContent, choices);
        this.nodes.Add(dialogueNode);
    }

    /// <summary>
    /// A transient inner builder that accumulates choices for a single dialogue node and commits the node to the
    /// parent <see cref="NodeBuilder{TRegistryKey,TUserId,TDialogueContent,TChoiceContent}"/> when
    /// <see cref="EndNode"/> is called.
    /// </summary>
    private sealed class ChoiceBuilder : IChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>
    {
        private readonly INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> parent;
        private readonly TUserId userId;
        private readonly TDialogueContent dialogueContent;
        private readonly List<Choice<TChoiceContent>> choices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceBuilder"/> class.
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
        public ChoiceBuilder(
            INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> parent,
            TUserId userId,
            TDialogueContent dialogueContent)
        {
            this.parent = parent;
            this.userId = userId;
            this.dialogueContent = dialogueContent;
            this.choices = [];
        }

        /// <inheritdoc/>
        public IChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> Choice(
            TUserId targetUserId,
            TChoiceContent choiceContent,
            ICondition? condition = null,
            IAction? action = null)
        {
            NodeId targetId = this.parent.GetInternalId(targetUserId);
            var choice = new Choice<TChoiceContent>(choiceContent, targetId, condition, action);
            this.choices.Add(choice);
            return this;
        }

        /// <inheritdoc/>
        public IChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> NoTargetChoice(
            TChoiceContent choiceContent,
            ICondition? condition = null,
            IAction? action = null)
        {
            var choice = new Choice<TChoiceContent>(choiceContent, null, condition, action);
            this.choices.Add(choice);
            return this;
        }

        /// <inheritdoc/>
        public INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> LastChoice(
            TUserId targetUserId,
            TChoiceContent choiceContent,
            ICondition? condition = null,
            IAction? action = null)
        {
            this.Choice(targetUserId, choiceContent, condition, action);
            this.parent.AddDialogueNodeInternal(this.userId, this.dialogueContent, this.choices);
            return this.parent;
        }

        /// <inheritdoc/>
        public INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> LastNoTargetChoice(
            TChoiceContent choiceContent,
            ICondition? condition = null,
            IAction? action = null)
        {
            this.NoTargetChoice(choiceContent, condition, action);
            this.parent.AddDialogueNodeInternal(this.userId, this.dialogueContent, this.choices);
            return this.parent;
        }
    }
}
