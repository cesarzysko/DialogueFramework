// <copyright file="ICondition.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An interface to define certain conditions which have to be met in order for a dialogue choice to be available.
/// </summary>
public interface ICondition
{
    /// <summary>
    /// Evaluates whether the condition is met.
    /// </summary>
    /// <param name="variables">The variables source used to determine whether the condition is met.</param>
    /// <returns>Whether the condition is met or not.</returns>
    bool Evaluate(IVariableStore? variables);
}
