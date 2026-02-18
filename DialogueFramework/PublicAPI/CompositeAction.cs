// <copyright file="CompositeAction.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Action that executes multiple actions in a sequence.
/// </summary>
public class CompositeAction : IAction
{
    private readonly IAction[] actions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeAction"/> class.
    /// </summary>
    /// <param name="actions">The actions to execute.</param>
    public CompositeAction(params IAction[] actions)
    {
        this.actions = actions;
    }

    /// <inheritdoc/>
    public void Execute(IVariableStore? variables)
    {
        foreach (var action in this.actions)
        {
            action.Execute(variables);
        }
    }
}
