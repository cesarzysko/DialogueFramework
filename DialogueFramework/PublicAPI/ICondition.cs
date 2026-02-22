// <copyright file="ICondition.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Encapsulates a predicate that determines whether a dialogue choice is available for selection.
/// </summary>
/// <typeparam name="TRegistryKey">
/// The key type used to identify values in the <see cref="IValueRegistry{TKey}"/>.
/// </typeparam>
public interface ICondition<out TRegistryKey>
    where TRegistryKey : notnull
{
    // TODO: Implement IReadOnlyValueRegistry for condition evaluation.

    /// <summary>
    /// Evaluates whether the condition is currently satisfied.
    /// </summary>
    /// <param name="valueRegistry">
    /// The registry to read values from.
    /// </param>
    /// <returns>
    /// true if the associated choice should be offered to the user;
    /// false if it should be hidden from the available choices.
    /// </returns>
    bool Evaluate(IValueRegistry<TRegistryKey>? valueRegistry);
}
