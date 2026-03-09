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
        var targetResolver = this.ToTargetResolver(targetUserId);
        return this.AddLinearOrTerminalNode(userId, dialogueContent, choiceContent, targetResolver, action: action);
    }

    /// <inheritdoc/>
    public ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddDynamicLinearNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TChoiceContent? choiceContent = default,
        IAction? action = null)
    {
        var choiceBuilder = new ChoiceBuilder(this, userId, dialogueContent);
        return new LastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>(
            choiceBuilder,
            this.GetNodeId,
            choiceContent,
            null,
            action);
    }

    /// <inheritdoc/>
    public INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddTerminalNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TChoiceContent? choiceContent = default,
        IAction? action = null)
    {
        return this.AddLinearOrTerminalNode(userId, dialogueContent, choiceContent, null, action: action);
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

        NodeId startNodeId = this.GetNodeId(startNode);
        return new Runner<TRegistryKey, TDialogueContent, TChoiceContent>(graph, valueRegistry, startNodeId);
    }

    /// <inheritdoc/>
    public void AddNodeInternal(
        TUserId userId,
        TDialogueContent dialogueContent,
        IReadOnlyList<IChoice<TChoiceContent>> choices)
    {
        NodeId id = this.GetNodeId(userId);
        var dialogueNode = new Node<TDialogueContent, TChoiceContent>(id, dialogueContent, choices);
        this.nodes.Add(dialogueNode);
    }

    /// <inheritdoc/>
    public ITargetResolver ToTargetResolver(TUserId userId)
    {
        NodeId internalId = this.GetNodeId(userId);
        TargetEntry entry = new TargetEntry(null, internalId);
        ITargetResolver resolver = new TargetResolver([entry]);
        return resolver;
    }

    /// <inheritdoc/>
    public NodeId GetNodeId(TUserId userId)
    {
        return this.registry.GetOrRegister(userId);
    }

    private NodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddLinearOrTerminalNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TChoiceContent? choiceContent,
        ITargetResolver? targetResolver,
        IAction? action)
    {
        var choice = new Choice<TChoiceContent>(choiceContent, targetResolver, action: action);
        this.AddNodeInternal(userId, dialogueContent, [choice]);
        return this;
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
        private readonly List<IChoice<TChoiceContent>> choices = [];

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
        }

        /// <inheritdoc/>
        public IChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> Choice(
            TUserId targetUserId,
            TChoiceContent choiceContent,
            ICondition? condition = null,
            IAction? action = null)
        {
            var targetResolver = this.parent.ToTargetResolver(targetUserId);
            return this.AddChoice(choiceContent, targetResolver, condition, action);
        }

        /// <inheritdoc/>
        public ITargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> DynamicChoice(
            TChoiceContent choiceContent,
            ICondition? condition = null,
            IAction? action = null)
        {
            return new TargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>(
                this,
                this.parent.GetNodeId,
                choiceContent,
                condition,
                action);
        }

        /// <inheritdoc/>
        public IChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> NoTargetChoice(
            TChoiceContent choiceContent,
            ICondition? condition = null,
            IAction? action = null)
        {
            return this.AddChoice(choiceContent, null, condition, action);
        }

        /// <inheritdoc/>
        public INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> LastChoice(
            TUserId targetUserId,
            TChoiceContent choiceContent,
            ICondition? condition = null,
            IAction? action = null)
        {
            this.Choice(targetUserId, choiceContent, condition, action);
            return this.EndNode();
        }

        /// <inheritdoc/>
        public ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> LastDynamicChoice(
            TChoiceContent choiceContent,
            ICondition? condition = null,
            IAction? action = null)
        {
            return new LastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>(
                this,
                this.parent.GetNodeId,
                choiceContent,
                condition,
                action);
        }

        /// <inheritdoc/>
        public INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> LastNoTargetChoice(
            TChoiceContent choiceContent,
            ICondition? condition = null,
            IAction? action = null)
        {
            this.NoTargetChoice(choiceContent, condition, action);
            return this.EndNode();
        }

        /// <inheritdoc/>
        public void AddChoiceInternal(
            TChoiceContent? content,
            ITargetResolver? targetResolver = null,
            ICondition? condition = null,
            IAction? action = null)
        {
            this.AddChoice(content, targetResolver, condition, action);
        }

        /// <inheritdoc/>
        public INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> EndNode()
        {
            this.parent.AddNodeInternal(this.userId, this.dialogueContent, this.choices);
            return this.parent;
        }

        private ChoiceBuilder AddChoice(
            TChoiceContent? content,
            ITargetResolver? targetResolver = null,
            ICondition? condition = null,
            IAction? action = null)
        {
            var choice = new Choice<TChoiceContent>(content, targetResolver, condition, action);
            this.choices.Add(choice);
            return this;
        }
    }
}
