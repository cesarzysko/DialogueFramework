// <copyright file="ReadWriteValueHandle.cs" company="SPS">
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
public sealed class ReadWriteValueHandle<TValue> : ReadOnlyValueHandle<TValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadWriteValueHandle{TValue}"/> class.
    /// </summary>
    /// <param name="value">
    /// The initial value.
    /// </param>
    internal ReadWriteValueHandle(TValue value)
        : base(value)
    {
    }

    /// <inheritdoc cref="ReadOnlyValueHandle{TValue}.WriteValue"/>
    internal void Write(TValue value)
    {
        this.WriteValue(value);
    }
}
