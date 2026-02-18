// <copyright file="IAction.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An interface to define certain actions to be performed upon selecting a dialogue choice.
/// </summary>
public interface IAction
{
    /// <summary>
    /// Executes actions tied to a dialogue choice selection.
    /// </summary>
    /// <param name="variables">The variables source used to perform the action execution.</param>
    void Execute(IVariableStore? variables);
}
