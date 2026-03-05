// <copyright file="ITargetResolver.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Interface used to define custom choice target node resolver.
/// </summary>
/// <typeparam name="TUserId">
/// The user-defined type used to name and reference dialogue nodes.
/// </typeparam>
public interface ITargetResolver<out TUserId>
{
    /// <summary>
    /// Determines the target node of the choice.
    /// </summary>
    /// <param name="registry">
    /// The read-only value registry used when determining the target node of the choice.
    /// </param>
    /// <returns>
    /// The user-defined target node id.
    /// </returns>
    public TUserId Resolve(IReadOnlyValueRegistry? registry);
}
