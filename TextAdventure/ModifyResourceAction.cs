// <copyright file="ModifyResourceAction.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework.Tests.XUnit;

/// <summary>
/// Action that modifies a player resource.
/// </summary>
public class ModifyResourceAction : IAction
{
    private readonly ResourceType resourceType;
    private readonly int amount;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifyResourceAction"/> class.
    /// </summary>
    /// <param name="resourceType">The resource to modify.</param>
    /// <param name="amount">The amount of resource to modify.</param>
    public ModifyResourceAction(ResourceType resourceType, int amount)
    {
        this.resourceType = resourceType;
        this.amount = amount;
    }

    /// <exception cref="ArgumentOutOfRangeException">Throws when the provided resource type is not handled by this action.</exception>
    /// <inheritdoc/>
    public void Execute(IVariableStore? variables)
    {
        if (variables == null)
        {
            return;
        }

        if (this.resourceType is not (ResourceType.Health or ResourceType.Mana or ResourceType.Gold))
        {
            return;
        }

        if (!variables.TryGet(this.resourceType, out int resourceValue))
        {
            return;
        }

        variables.TrySet(this.resourceType, resourceValue + this.amount);
    }
}
