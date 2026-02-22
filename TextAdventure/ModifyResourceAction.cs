// <copyright file="ModifyResourceAction.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework.Tests.XUnit;

/// <summary>
/// Action that modifies a player resource.
/// </summary>
/// <typeparam name="TRegistryKey">The type key used to identify values in the registry.</typeparam>
public class ModifyResourceAction<TRegistryKey> : IAction<TRegistryKey>
    where TRegistryKey : notnull
{
    private readonly ValueHandle<int> resourceHandle;
    private readonly int amount;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifyResourceAction{TRegistryKey}"/> class.
    /// </summary>
    /// <param name="resourceHandle">The resource to modify.</param>
    /// <param name="amount">The amount of resource to modify.</param>
    public ModifyResourceAction(ValueHandle<int> resourceHandle, int amount)
    {
        this.resourceHandle = resourceHandle;
        this.amount = amount;
    }

    /// <inheritdoc/>
    public void Execute(IValueRegistry<TRegistryKey>? valueRegistry)
    {
        valueRegistry?.Set(this.resourceHandle, valueRegistry.Get(this.resourceHandle) + this.amount);
    }
}
