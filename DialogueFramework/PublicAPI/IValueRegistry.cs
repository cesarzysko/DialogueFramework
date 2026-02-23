// <copyright file="IValueRegistry.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// A typed value registry that conditions and actions use to read and write shared state during dialogue traversal.
/// </summary>
/// <typeparam name="TKey">
/// The type used to identify a registry entry at registration time.
/// </typeparam>
public interface IValueRegistry<in TKey> : IReadWriteValueRegistry
    where TKey : notnull
{
    /// <summary>
    /// Registers a new entry in the registry and returns a typed key for accessing it.
    /// </summary>
    /// <param name="key">
    /// A user-defined identifier for the new entry. Must be unique within this registry.
    /// </param>
    /// <param name="initialValue">
    /// The value the entry holds immediately after registration.
    /// </param>
    /// <typeparam name="TValue">
    /// The type of value to store under this entry.
    /// </typeparam>
    /// <returns>
    /// A <see cref="ValueHandle{TValue}"/> that uniquely identifies this entry.
    /// Store this key to use with <see cref="Get{TValue}"/> and <see cref="Set{TValue}"/>.
    /// </returns>
    public ValueHandle<TValue> Register<TValue>(TKey key, TValue initialValue = default!);
}
