// <copyright file="HasMinimumResourceCondition.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework.Tests.XUnit;

/// <summary>
/// Condition that checks if the player has minimum required resources.
/// </summary>
/// <typeparam name="TRegistryKey">The type key used to identify values in the registry.</typeparam>
public class HasMinimumResourceCondition<TRegistryKey> : ICondition<TRegistryKey>
    where TRegistryKey : notnull
{
    private readonly ValueHandle<int> resourceHandle;
    private readonly int minimumAmount;

    /// <summary>
    /// Initializes a new instance of the <see cref="HasMinimumResourceCondition{TRegistryKey}"/> class.
    /// </summary>
    /// <param name="resourceHandle">The type of resource checked.</param>
    /// <param name="minimumAmount">The minimum amount of the resource the player needs to have.</param>
    public HasMinimumResourceCondition(ValueHandle<int> resourceHandle, int minimumAmount)
    {
        this.resourceHandle = resourceHandle;
        this.minimumAmount = minimumAmount;
    }

    /// <inheritdoc/>
    public bool Evaluate(IValueRegistry<TRegistryKey>? valueRegistry)
    {
        if (valueRegistry == null)
        {
            return false;
        }

        return valueRegistry.Get(this.resourceHandle) >= this.minimumAmount;
    }
}
