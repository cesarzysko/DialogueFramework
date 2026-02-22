// <copyright file="IAction.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Encapsulates an effect to be carried out when a dialogue choice is selected.
/// </summary>
/// <typeparam name="TRegistryKey">
/// The key type used to identify values in the <see cref="IValueRegistry{TKey}"/>.
/// </typeparam>
public interface IAction<out TRegistryKey>
    where TRegistryKey : notnull
{
    // TODO: Implement IReadWriteValueRegistry for action execution.

    /// <summary>
    /// Carries out the effect associated with this action.
    /// </summary>
    /// <param name="valueRegistry">
    /// The registry to read from or write to.
    /// </param>
    void Execute(IValueRegistry<TRegistryKey>? valueRegistry);
}
