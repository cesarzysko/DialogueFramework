// <copyright file="HasMinimumResourceCondition.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework.Tests.XUnit;

/// <summary>
/// Condition that checks if the player has minimum required resources.
/// </summary>
public class HasMinimumResourceCondition : ICondition
{
    private readonly ResourceType resourceType;
    private readonly int minimumAmount;

    /// <summary>
    /// Initializes a new instance of the <see cref="HasMinimumResourceCondition"/> class.
    /// </summary>
    /// <param name="resourceType">The type of resource checked.</param>
    /// <param name="minimumAmount">The minimum amount of the resource the player needs to have.</param>
    public HasMinimumResourceCondition(ResourceType resourceType, int minimumAmount)
    {
        this.resourceType = resourceType;
        this.minimumAmount = minimumAmount;
    }

    /// <inheritdoc/>
    public bool Evaluate(IVariableStore? variables)
    {
        if (variables == null)
        {
            return false;
        }

        return this.resourceType switch
        {
            ResourceType.Health => variables.TryGet(ResourceType.Health, out int health) && health >= this.minimumAmount,
            ResourceType.Mana => variables.TryGet(ResourceType.Mana, out int mana) && mana >= this.minimumAmount,
            ResourceType.Gold => variables.TryGet(ResourceType.Gold, out int gold) && gold >= this.minimumAmount,
            _ => false,
        };
    }
}
