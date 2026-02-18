// <copyright file="DialogueNodeBuilder.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// A common class for building dialogue nodes without having to explicitly handle the registry.
/// </summary>
/// <param name="logger">The optional logger to use to print internal messages.</param>
/// <typeparam name="TUserId">The user-front id for dialogue nodes.</typeparam>
/// <typeparam name="TDialogueContent">The displayable content of dialogue nodes.</typeparam>
/// <typeparam name="TChoiceContent">The displayable content of dialogue choices.</typeparam>
internal sealed class DialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent>(
    ILogger? logger)
    : IDialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent>
    where TUserId : notnull
{
    private readonly List<DialogueNode<TDialogueContent, TChoiceContent>> nodes = [];

    private readonly NodeIdRegistry<TUserId> registry = new(logger);

    /// <inheritdoc/>
    public IDialogueRunner<TDialogueContent, TChoiceContent> Build(IVariableStore? variableStore, TUserId startNode)
    {
        if (this.nodes.Count == 0)
        {
            throw new InvalidOperationException("Cannot build dialogue with no nodes.");
        }

        var graph = new DialogueGraph<TDialogueContent, TChoiceContent>(this.nodes);
        return new DialogueRunner<TDialogueContent, TChoiceContent>(graph, variableStore, this.registry.GetInternalId(startNode));
    }

    /// <inheritdoc/>
    public IDialogueNodeChoiceBuilder<TUserId, TDialogueContent, TChoiceContent> AddMultiChoiceNode(
        TUserId userId,
        TDialogueContent dialogueContent)
    {
        return new DialogueNodeChoiceBuilder(this, userId, dialogueContent);
    }

    /// <inheritdoc/>
    public IDialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent> AddLinearNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TUserId targetUserId,
        TChoiceContent? choiceContent = default,
        IAction? action = null)
    {
        return this.AddMultiChoiceNode(userId, dialogueContent)
            .WithChoice(targetUserId, choiceContent!, null, action)
            .EndNode();
    }

    /// <inheritdoc/>
    public IDialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent> AddTerminalNode(
        TUserId userId,
        TDialogueContent dialogueContent,
        TChoiceContent? choiceContent = default,
        IAction? action = null)
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
        IReadOnlyList<DialogueChoice<TChoiceContent>> choices)
    {
        NodeId id = this.registry.GetOrRegister(userId);
        var dialogueNode = new DialogueNode<TDialogueContent, TChoiceContent>(id, dialogueContent, choices);
        this.nodes.Add(dialogueNode);
    }

    private sealed class DialogueNodeChoiceBuilder : IDialogueNodeChoiceBuilder<TUserId, TDialogueContent, TChoiceContent>
    {
        private readonly IDialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent> parent;
        private readonly TUserId userId;
        private readonly TDialogueContent dialogueContent;
        private readonly List<DialogueChoice<TChoiceContent>> choices;

        public DialogueNodeChoiceBuilder(
            IDialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent> parent,
            TUserId userId,
            TDialogueContent dialogueContent)
        {
            this.parent = parent;
            this.userId = userId;
            this.dialogueContent = dialogueContent;
            this.choices = [];
        }

        public IDialogueNodeChoiceBuilder<TUserId, TDialogueContent, TChoiceContent> WithChoice(
            TUserId targetUserId,
            TChoiceContent choiceContent,
            ICondition? condition = null,
            IAction? action = null)
        {
            NodeId targetId = this.parent.GetInternalId(targetUserId);

            var choice = new DialogueChoice<TChoiceContent>(choiceContent, targetId, condition, action);
            this.choices.Add(choice);

            return this;
        }

        public IDialogueNodeChoiceBuilder<TUserId, TDialogueContent, TChoiceContent> WithEndChoice(
            TChoiceContent? choiceContent,
            ICondition? condition = null,
            IAction? action = null)
        {
            var choice = new DialogueChoice<TChoiceContent>(choiceContent, null, condition, action);
            this.choices.Add(choice);

            return this;
        }

        public IDialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent> EndNode()
        {
            this.parent.AddDialogueNodeInternal(this.userId, this.dialogueContent, this.choices);
            return this.parent;
        }
    }
}
