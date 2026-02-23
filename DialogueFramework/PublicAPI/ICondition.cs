// <copyright file="ICondition.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Encapsulates a predicate that determines whether a dialogue choice is available for selection.
/// </summary>
public interface ICondition
{
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
    bool Evaluate(IReadOnlyValueRegistry? valueRegistry);
}
