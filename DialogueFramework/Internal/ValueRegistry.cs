// <copyright file="ValueRegistry.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// The default implementation of <see cref="IValueRegistry{TKey}"/>, storing values in a dictionary keyed by
/// <see cref="ValueHandle{TValue}"/> instances.
/// </summary>
/// <typeparam name="TKey">
/// The user-defined type used to name a registry entry at registration time.
/// </typeparam>
internal sealed class ValueRegistry<TKey> : IValueRegistry<TKey>
    where TKey : notnull
{
    // TODO: Add a logger field and use it to log exceptions.
    private readonly HashSet<TKey> registeredKeys = [];
    private readonly Dictionary<object, object?> variables = [];

    /// <inheritdoc/>
    public ValueHandle<TValue> Register<TValue>(TKey key, TValue initialValue = default!)
    {
        if (!this.registeredKeys.Add(key))
        {
            throw new ArgumentException($"The key \"{key}\" is already registered.", nameof(key));
        }

        var variableKey = new ValueHandle<TValue>();
        this.variables.Add(variableKey, initialValue);
        return variableKey;
    }

    /// <inheritdoc/>
    public TValue Get<TValue>(ValueHandle<TValue> handle)
    {
        if (!this.variables.TryGetValue(handle, out object? obj))
        {
            throw new ArgumentException("The provided key is not present in the registry.", nameof(handle));
        }

        if (obj is TValue typedValue)
        {
            return typedValue;
        }

        // TODO: Use typed object wrappers to store values in the dictionary for better type checks.
        if (obj != null)
        {
            throw new ArgumentException("The provided key does not match the requested value type.", nameof(handle));
        }

        return default!;
    }

    /// <inheritdoc/>
    public void Set<TValue>(ValueHandle<TValue> handle, TValue value)
    {
        if (!this.variables.ContainsKey(handle))
        {
            throw new ArgumentException("The provided key is not present in the registry.", nameof(handle));
        }

        // TODO: Use typed object wrappers to store values in the dictionary for better type checks.
        this.variables[handle] = value;
    }
}
