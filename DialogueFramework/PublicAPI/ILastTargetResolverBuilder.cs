// <copyright file="ILastTargetResolverBuilder.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Interface for choice target resolver building used when no more choices are expected after this one.
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
public interface ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent>
    where TRegistryKey : notnull
    where TUserId : notnull
{
    /// <summary>
    /// Adds a new target with a conditions which must be met.
    /// </summary>
    /// <param name="condition">
    /// The condition which must be met for this target to be valid.
    /// </param>
    /// <param name="targetUserId">
    /// The user-defined identification of the target node.
    /// </param>
    /// <returns>
    /// The instance of this builder.
    /// </returns>
    public ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddTarget(
        ICondition condition,
        TUserId targetUserId);

    /// <summary>
    /// Adds a new terminal target with a condition which must be met.
    /// </summary>
    /// <param name="condition">
    /// The condition which must be met for this target to be valid.
    /// </param>
    /// <returns>
    /// The instance of this builder.
    /// </returns>
    public ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddTerminal(
        ICondition condition);

    /// <summary>
    /// Adds a new fallback target without any condition to be met.
    /// </summary>
    /// <param name="targetUserId">
    /// The user-defined identification of the target node.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Throws when an unconditional fallback already exists.
    /// </exception>
    /// <returns>
    /// The instance of this builder.
    /// </returns>
    public ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddFallback(
        TUserId targetUserId);

    /// <summary>
    /// Adds a new terminal fallback target without any condition to be met.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Throws when an unconditional fallback already exists.
    /// </exception>
    /// /// <returns>
    /// The instance of this builder.
    /// </returns>
    public ILastTargetResolverBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> AddTerminalFallback();

    /// <summary>
    /// Ends the creation of the target resolver.
    /// </summary>
    /// <returns>
    /// The node builder used to create more nodes.
    /// </returns>
    public INodeBuilder<TRegistryKey, TUserId, TDialogueContent, TChoiceContent> End();
}