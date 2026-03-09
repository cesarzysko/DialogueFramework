// <copyright file="IChoiceBuilder.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// A transient builder for defining the choices of a single dialogue node.
/// </summary>
/// <typeparam name="TRegistryKey">
/// The key type used to identify values in the <see cref="IValueRegistry{TKey}"/>.
/// </typeparam>
/// <typeparam name="TUserId">
/// The user-defined type used to identify dialogue nodes.
/// </typeparam>
/// <typeparam name="TDialogueContent">
/// The type of displayable data on a dialogue node.
/// </typeparam>
/// <typeparam name="TChoiceContent">
/// The type of displayable data on a dialogue choice.
/// </typeparam>
public interface IChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>
    where TUserId : notnull
    where TRegistryKey : notnull
{
    /// <summary>
    /// Adds a choice that, when selected, advances the dialogue to the specified target node.
    /// </summary>
    /// <param name="targetUserId">
    /// The user-defined identifier of the node this choice leads to.
    /// </param>
    /// <param name="choiceContent">
    /// The data to display when presenting this choice to the user.
    /// </param>
    /// <param name="condition">
    /// An optional predicate evaluated to determine whether the choice is currently available.
    /// </param>
    /// <param name="action">
    /// An optional side effect executed when the user selects this choice.
    /// </param>
    /// <returns>
    /// This builder instance, enabling method chaining.
    /// </returns>
    public IChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> Choice(
        TUserId targetUserId,
        TChoiceContent choiceContent,
        ICondition? condition = null,
        IAction? action = null);

    /// <summary>
    /// Adds a choice that, when selected, advances the dialogue to the specified target node.
    /// </summary>
    /// <param name="choiceContent">
    /// The data to display when presenting this choice to the user.
    /// </param>
    /// <param name="condition">
    /// An optional predicate evaluated to determine whether the choice is currently available.
    /// </param>
    /// <param name="action">
    /// An optional side effect executed when the user selects this choice.
    /// </param>
    /// <returns>
    /// The target resolver builder instance for creating target routing.
    /// </returns>
    public ITargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> DynamicChoice(
        TChoiceContent choiceContent,
        ICondition? condition = null,
        IAction? action = null);

    /// <summary>
    /// Adds a choice that, when selected, ends the dialogue.
    /// </summary>
    /// <param name="choiceContent">
    /// The data to display when presenting this choice to the user.
    /// </param>
    /// <param name="condition">
    /// An optional predicate evaluated to determine whether the choice is currently available.
    /// </param>
    /// <param name="action">
    /// An optional side effect executed when the user selects this choice.
    /// </param>
    /// <returns>
    /// This builder instance, enabling method chaining.
    /// </returns>
    public IChoiceBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> NoTargetChoice(
        TChoiceContent choiceContent,
        ICondition? condition = null,
        IAction? action = null);

    /// <summary>
    /// Adds a choice that, when selected, advances the dialogue to the specified target node.
    /// Returns the parent builder to continue defining further nodes.
    /// </summary>
    /// <param name="targetUserId">
    /// The user-defined identifier of the node this choice leads to.
    /// </param>
    /// <param name="choiceContent">
    /// The data to display when presenting this choice to the user.
    /// </param>
    /// <param name="condition">
    /// An optional predicate evaluated to determine whether the choice is currently available.
    /// </param>
    /// <param name="action">
    /// An optional side effect executed when the user selects this choice.
    /// </param>
    /// <returns>
    /// The <see cref="INodeBuilder{TRegistryKey,TUserId,TDialogueContent,TChoiceContent}"/> that created this
    /// choice builder.
    /// </returns>
    public INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> LastChoice(
        TUserId targetUserId,
        TChoiceContent choiceContent,
        ICondition? condition = null,
        IAction? action = null);

    /// <summary>
    /// Adds a choice that, when selected, advances the dialogue to the specified target node.
    /// Returns the parent builder to continue defining further nodes.
    /// </summary>
    /// <param name="choiceContent">
    /// The data to display when presenting this choice to the user.
    /// </param>
    /// <param name="condition">
    /// An optional predicate evaluated to determine whether the choice is currently available.
    /// </param>
    /// <param name="action">
    /// An optional side effect executed when the user selects this choice.
    /// </param>
    /// <returns>
    /// The target resolver builder instance for creating target routing.
    /// </returns>
    public ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> LastDynamicChoice(
        TChoiceContent choiceContent,
        ICondition? condition = null,
        IAction? action = null);

    /// <summary>
    /// Adds a choice that, when selected, ends the dialogue.
    /// Returns the parent builder to continue defining further nodes.
    /// </summary>
    /// <param name="choiceContent">
    /// The data to display when presenting this choice to the user.
    /// </param>
    /// <param name="condition">
    /// An optional predicate evaluated to determine whether the choice is currently available.
    /// </param>
    /// <param name="action">
    /// An optional side effect executed when the user selects this choice.
    /// </param>
    /// <returns>
    /// The <see cref="INodeBuilder{TRegistryKey,TUserId,TDialogueContent,TChoiceContent}"/> that created this
    /// choice builder.
    /// </returns>
    public INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> LastNoTargetChoice(
        TChoiceContent choiceContent,
        ICondition? condition = null,
        IAction? action = null);

    /// <summary>
    /// Ends the process of building choices for the current node.
    /// </summary>
    /// <returns>
    /// The <see cref="INodeBuilder{TRegistryKey,TUserId,TDialogueContent,TChoiceContent}"/> that created this
    /// choice builder.
    /// </returns>
    internal INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> EndNode();

    /// <summary>
    /// Adds a new choice to the list of choices.
    /// </summary>
    /// <param name="content">
    /// The displayable content of the choice.
    /// </param>
    /// <param name="targetResolver">
    /// The target resolver used to get the target node id.
    /// </param>
    /// <param name="condition">
    /// The optional condition that must be met for this option to appear.
    /// </param>
    /// <param name="action">
    /// The optional action to be executed once this option is chosen.
    /// </param>
    internal void AddChoiceInternal(
        TChoiceContent? content,
        ITargetResolver? targetResolver = null,
        ICondition? condition = null,
        IAction? action = null);
}
