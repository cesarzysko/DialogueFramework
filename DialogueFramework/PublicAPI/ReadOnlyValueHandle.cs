// <copyright file="ReadOnlyValueHandle.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// A type-safe handle returned by <see cref="IValueRegistry{TKey}"/> upon registering a value, and used for all
/// subsequent reads of that value.
/// </summary>
/// <typeparam name="TValue">
/// The type of the value this key provides access to.
/// </typeparam>
public class ReadOnlyValueHandle<TValue>
{
    private readonly ValueWrapper<TValue> wrappedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyValueHandle{TValue}"/> class.
    /// </summary>
    /// <param name="value">
    /// The initial value.
    /// </param>
    internal ReadOnlyValueHandle(TValue value)
    {
        this.wrappedValue = new ValueWrapper<TValue>(value);
    }

    /// <summary>
    /// Reads the internally stored value inside the wrapper.
    /// </summary>
    /// <returns>
    /// Value stored by the handle.
    /// </returns>
    internal TValue Read()
    {
        return this.wrappedValue.Value;
    }

    /// <summary>
    /// Writes to the internally stored value inside the wrapper.
    /// </summary>
    /// <param name="value">
    /// The new value stored by the handle.
    /// </param>
    protected void WriteValue(TValue value)
    {
        this.wrappedValue.Value = value;
    }
}
