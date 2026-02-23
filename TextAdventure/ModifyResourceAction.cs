// <copyright file="ModifyResourceAction.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework.Tests.XUnit;

/// <summary>
/// Action that modifies a player resource.
/// </summary>
public class ModifyResourceAction : IAction
{
    private readonly ValueHandle<int> resourceHandle;
    private readonly int amount;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifyResourceAction"/> class.
    /// </summary>
    /// <param name="resourceHandle">
    /// The resource to modify.
    /// </param>
    /// <param name="amount">
    /// The amount of resource to modify.
    /// </param>
    public ModifyResourceAction(ValueHandle<int> resourceHandle, int amount)
    {
        this.resourceHandle = resourceHandle;
        this.amount = amount;
    }

    /// <inheritdoc/>
    public void Execute(IReadWriteValueRegistry? valueRegistry)
    {
        valueRegistry?.Set(this.resourceHandle, valueRegistry.Get(this.resourceHandle) + this.amount);
    }
}
