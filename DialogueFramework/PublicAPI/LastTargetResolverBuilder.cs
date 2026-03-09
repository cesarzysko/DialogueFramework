// <copyright file="LastTargetResolverBuilder.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Target Resolver Builder used when no more choices are expected after this one.
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
public sealed class LastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>
    : TargetResolverBuilderBase<TUserId>,
      ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>
    where TRegistryKey : notnull
    where TUserId : notnull
{
    private readonly IChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> parent;
    private readonly Func<TUserId, NodeId> toNodeIdFunc;
    private readonly TChoiceContent? cachedChoiceContent;
    private readonly ICondition? cachedCondition;
    private readonly IAction? cachedAction;

    /// <summary>
    /// Initializes a new instance of the <see cref="LastTargetResolverBuilder{TRegistryKey, TUserId, TDialogueContent, TChoiceContent}"/> class.
    /// </summary>
    /// <param name="parent">
    /// The parent <see cref="IChoiceBuilder{TRegistryKey,TUserId,TDialogueContent,TChoiceContent}"/> which created
    /// this instance.
    /// </param>
    /// <param name="toNodeIdFunc">
    /// Function which returns the NodeId correlating to the given user-defined identification.
    /// </param>
    /// <param name="choiceContent">
    /// The displayable content of the choice.
    /// </param>
    /// <param name="condition">
    /// The condition that must be met in order for the choice to be available.
    /// </param>
    /// <param name="action">
    /// The action to be executed once the choice is chosen.
    /// </param>
    internal LastTargetResolverBuilder(
        IChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> parent,
        Func<TUserId, NodeId> toNodeIdFunc,
        TChoiceContent? choiceContent,
        ICondition? condition = null,
        IAction? action = null)
    {
        this.parent = parent;
        this.toNodeIdFunc = toNodeIdFunc;

        this.cachedChoiceContent = choiceContent;
        this.cachedCondition = condition;
        this.cachedAction = action;
    }

    /// <inheritdoc/>
    public new ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddTarget(
        ICondition condition,
        TUserId targetUserId)
    {
        base.AddTarget(condition, targetUserId);
        return this;
    }

    /// <inheritdoc/>
    public new ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddTerminal(
        ICondition condition)
    {
        base.AddTerminal(condition);
        return this;
    }

    /// <inheritdoc/>
    public new ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddFallback(
        TUserId targetUserId)
    {
        base.AddFallback(targetUserId);
        return this;
    }

    /// <inheritdoc/>
    public new ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddTerminalFallback()
    {
        base.AddTerminalFallback();
        return this;
    }

    /// <inheritdoc/>
    public INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> End()
    {
        var resolver = new TargetResolver(this.GetEntries());
        this.parent.AddChoiceInternal(this.cachedChoiceContent, resolver, this.cachedCondition, this.cachedAction);
        return this.parent.EndNode();
    }

    /// <inheritdoc/>
    private protected override NodeId ToNodeId(TUserId userId)
    {
        return this.toNodeIdFunc.Invoke(userId);
    }
}
