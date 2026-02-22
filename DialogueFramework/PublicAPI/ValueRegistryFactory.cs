// <copyright file="ValueRegistryFactory.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// A static class used to create a value registry.
/// </summary>
public static class ValueRegistryFactory
{
    /// <summary>
    /// Creates a value registry.
    /// </summary>
    /// <typeparam name="TKey">The type of key used to identify the values in the registry.</typeparam>
    /// <returns>The created value registry.</returns>
    public static IValueRegistry<TKey> CreateValueRegistry<TKey>()
        where TKey : notnull
    {
        return new ValueRegistry<TKey>();
    }
}
