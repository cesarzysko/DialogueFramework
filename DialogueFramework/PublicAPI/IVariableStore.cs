// <copyright file="IVariableStore.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An interface to define a variable store from which certain values can be retrieved or set.
/// </summary>
public interface IVariableStore
{
    /// <summary>
    /// Attempts to retrieve a value identified by the given key.
    /// </summary>
    /// <param name="key">The key used to identify a value.</param>
    /// <param name="value">The value to which the provided key points.</param>
    /// <typeparam name="TKey">The type of the key used to identify the value.</typeparam>
    /// <typeparam name="TValue">The type of the value returned.</typeparam>
    /// <returns>Whether the value was successfully retrieved or not.</returns>
    bool TryGet<TKey, TValue>(TKey key, out TValue value);

    /// <summary>
    /// Attempts to set a value identified by the given key.
    /// </summary>
    /// <param name="key">The key used to identify a value.</param>
    /// <param name="value">The new value to set the target value to.</param>
    /// <typeparam name="TKey">The type of the key used to identify the value.</typeparam>
    /// <typeparam name="TValue">The type of the value to set.</typeparam>
    /// /// <returns>Whether the value was successfully set or not.</returns>
    bool TrySet<TKey, TValue>(TKey key, TValue value);
}
