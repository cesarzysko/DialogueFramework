// <copyright file="ValueHandle.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// A type-safe handle returned by <see cref="IValueRegistry{TKey}"/> upon registering a value, and used for all
/// subsequent reads and writes of that value.
/// </summary>
/// <typeparam name="TValue">
/// The type of the value this key provides access to.
/// </typeparam>
public sealed class ValueHandle<TValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValueHandle{TValue}"/> class.
    /// </summary>
    /// <param name="isReadOnly">
    /// Tells the <see cref="IValueRegistry{TKey}"/> whether to register a value as get-set or get-only.
    /// </param>
    internal ValueHandle(bool isReadOnly = false)
    {
        this.IsReadOnly = isReadOnly;
    }

    /// <summary>
    /// Gets a value indicating whether the registry value associated with this handle is get-only.
    /// </summary>
    // TODO: Implement readonly registry values.
    internal bool IsReadOnly { get; }
}
