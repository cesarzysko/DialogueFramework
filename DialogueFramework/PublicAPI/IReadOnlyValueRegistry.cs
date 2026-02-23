// <copyright file="IReadOnlyValueRegistry.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// A typed read-only value registry that conditions use to read shared state during dialogue traversal.
/// </summary>
public interface IReadOnlyValueRegistry
{
    /// <summary>
    /// Retrieves the value associated with the given key.
    /// </summary>
    /// <param name="handle">
    /// The key returned by <see cref="IValueRegistry{TKey}.RegisterReadWrite{TValue}"/> for the target entry.
    /// </param>
    /// <typeparam name="TValue">
    /// The type of the value to retrieve. Must match the type used at registration.
    /// </typeparam>
    /// <returns>
    /// The current value of the entry identified by <paramref name="handle"/>.
    /// </returns>
    public TValue Get<TValue>(ReadOnlyValueHandle<TValue> handle);
}
