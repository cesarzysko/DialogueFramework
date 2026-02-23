// <copyright file="ValueRegistry.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// The default implementation of <see cref="IValueRegistry{TKey}"/>, creating and managing the uniqueness of
/// registered values.
/// </summary>
/// <typeparam name="TKey">
/// The user-defined type used to name a registry entry at registration time.
/// </typeparam>
internal sealed class ValueRegistry<TKey> : IValueRegistry<TKey>
    where TKey : notnull
{
    private readonly ILogger? logger;
    private readonly HashSet<TKey> keys = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueRegistry{TKey}"/> class.
    /// </summary>
    /// <param name="logger">
    /// The optional logger to use to print internal diagnostics.
    /// </param>
    internal ValueRegistry(ILogger? logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc/>
    public ReadWriteValueHandle<TValue> RegisterReadWrite<TValue>(TKey key, TValue initialValue = default!)
    {
        if (this.keys.Add(key))
        {
            return new ReadWriteValueHandle<TValue>(initialValue);
        }

        var exc = new ArgumentException($"The key \"{key}\" is already registered.", nameof(key));
        this.logger?.LogError(exc.Message);
        throw exc;
    }

    /// <inheritdoc/>
    public ReadOnlyValueHandle<TValue> RegisterReadOnly<TValue>(TKey key, TValue initialValue)
        where TValue : notnull
    {
        if (this.keys.Add(key))
        {
            return new ReadOnlyValueHandle<TValue>(initialValue);
        }

        var exc = new ArgumentException($"The key \"{key}\" is already registered.", nameof(key));
        this.logger?.LogError(exc.Message);
        throw exc;
    }

    /// <inheritdoc/>
    public TValue Get<TValue>(ReadOnlyValueHandle<TValue> handle)
    {
        return handle.Read();
    }

    /// <inheritdoc/>
    public void Set<TValue>(ReadWriteValueHandle<TValue> handle, TValue value)
    {
        handle.Write(value);
    }
}
