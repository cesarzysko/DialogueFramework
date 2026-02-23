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
    /// <param name="logger">
    /// The optional logger to use to print internal diagnostics.
    /// </param>
    /// <typeparam name="TKey">
    /// The type of key used to identify the values in the registry.
    /// </typeparam>
    /// <returns>
    /// The created value registry.
    /// </returns>
    public static IValueRegistry<TKey> CreateValueRegistry<TKey>(ILogger? logger = null)
        where TKey : notnull
    {
        return new ValueRegistry<TKey>(logger);
    }
}
