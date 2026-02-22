// <copyright file="ValueRegistry.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework.Tests.XUnit;

/// <summary>
/// Creates a value registry and registers all required values.
/// </summary>
public static class ValueRegistry
{
    /// <summary>
    /// The registry holding all shared values accessible to the dialogue framework.
    /// </summary>
    public static readonly IValueRegistry<string> Registry = ValueRegistryFactory.CreateValueRegistry<string>();

    /// <summary>
    /// Handle used to retrieve or set health.
    /// </summary>
    public static readonly ValueHandle<int> Health = Registry.Register("Health", 100);

    /// <summary>
    /// Handle used to retrieve or set mana.
    /// </summary>
    public static readonly ValueHandle<int> Mana = Registry.Register("Mana", 100);

    /// <summary>
    /// Handle used to retrieve or set gold.
    /// </summary>
    public static readonly ValueHandle<int> Gold = Registry.Register("Gold", 100);
}
