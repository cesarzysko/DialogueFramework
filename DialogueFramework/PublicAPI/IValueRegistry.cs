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
public interface IValueRegistry<in TKey>
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

    /// <summary>
    /// Retrieves the value associated with the given key.
    /// </summary>
    /// <param name="handle">
    /// The key returned by <see cref="Register{TValue}"/> for the target entry.
    /// </param>
    /// <typeparam name="TValue">
    /// The type of the value to retrieve. Must match the type used at registration.
    /// </typeparam>
    /// <returns>
    /// The current value of the entry identified by <paramref name="handle"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="handle"/> does not correspond to any registered entry,
    /// or when <typeparamref name="TValue"/> does not match the type used at registration.
    /// </exception>
    public TValue Get<TValue>(ValueHandle<TValue> handle);

    /// <summary>
    /// Updates the value associated with the given key.
    /// </summary>
    /// <param name="handle">
    /// The key returned by <see cref="Register{TValue}"/> for the target entry.
    /// </param>
    /// <param name="value">
    /// The new value to assign to the entry.
    /// </param>
    /// <typeparam name="TValue">
    /// The type of the value to set. Must match the type used at registration.
    /// </typeparam>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="handle"/> does not correspond to any registered entry.
    /// </exception>
    public void Set<TValue>(ValueHandle<TValue> handle, TValue value);
}
