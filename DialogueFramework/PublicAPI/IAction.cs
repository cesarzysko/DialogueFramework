// <copyright file="IAction.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Encapsulates an effect to be carried out when a dialogue choice is selected.
/// </summary>
public interface IAction
{
    /// <summary>
    /// Carries out the effect associated with this action.
    /// </summary>
    /// <param name="valueRegistry">
    /// The registry to read from or write to.
    /// </param>
    void Execute(IReadWriteValueRegistry? valueRegistry);
}
