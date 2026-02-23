// <copyright file="ValueWrapper.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Wraps around a value, enabling type checks for null values.
/// </summary>
/// <typeparam name="TValue">
/// The value type to wrap around.
/// </typeparam>
internal sealed class ValueWrapper<TValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValueWrapper{TValue}"/> class.
    /// </summary>
    /// <param name="value">
    /// The initial wrapped value.
    /// </param>
    public ValueWrapper(TValue value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Gets or sets the wrapped value.
    /// </summary>
    public TValue Value { get; set; }
}
