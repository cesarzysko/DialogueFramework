// <copyright file="IReadWriteValueRegistry.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// A typed read-write value registry that actions use to read and write shared state during dialogue traversal.
/// </summary>
public interface IReadWriteValueRegistry : IReadOnlyValueRegistry
{
    /// <summary>
    /// Updates the value associated with the given key.
    /// </summary>
    /// <param name="handle">
    /// The key returned by <see cref="IValueRegistry{TKey}.Register{TValue}"/> for the target entry.
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
