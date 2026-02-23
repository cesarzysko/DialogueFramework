// <copyright file="NodeIdRegistry.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Maps user-defined node identifiers of type <typeparamref name="TUserId"/> to sequentially assigned internal
/// <see cref="NodeId"/> values, and supports reverse lookup.
/// </summary>
/// <typeparam name="TUserId">
/// The user-defined type used to name dialogue nodes.
/// </typeparam>
internal sealed class NodeIdRegistry<TUserId>
    where TUserId : notnull
{
    private int nextId;

    /// <summary>
    /// Initializes a new instance of the <see cref="NodeIdRegistry{TUserId}"/> class.
    /// </summary>
    /// <param name="logger">
    /// An optional logger that receives debug messages.
    /// </param>
    internal NodeIdRegistry(ILogger? logger = null)
    {
        this.Logger = logger;
    }

    private Dictionary<TUserId, NodeId> UserToInternal { get; } = new();

    private Dictionary<int, TUserId> InternalToUser { get; } = new();

    private ILogger? Logger { get; }

    /// <summary>
    /// Returns the internal <see cref="NodeId"/> assigned to <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">
    /// The user-defined identifier to look up.
    /// </param>
    /// <returns>
    /// The corresponding <see cref="NodeId"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="userId"/> has not been registered.
    /// </exception>
    public NodeId GetInternalId(TUserId userId)
    {
        if (this.UserToInternal.TryGetValue(userId, out NodeId internalId))
        {
            return internalId;
        }

        string msg = $"There is no internal id registered for {userId}.";
        this.Logger?.LogError(msg);
        throw new ArgumentException(msg, nameof(userId));
    }

    /// <summary>
    /// Attempts to retrieve the internal <see cref="NodeId"/> assigned to <paramref name="userId"/> without throwing
    /// if it is not found.
    /// </summary>
    /// <param name="userId">
    /// The user-defined identifier to look up.
    /// </param>
    /// <param name="internalId">
    /// The corresponding <see cref="NodeId"/>.
    /// </param>
    /// <returns>
    /// true if <paramref name="userId"/> is registered; otherwise false.
    /// </returns>
    public bool TryGetInternalId(TUserId userId, out NodeId internalId)
    {
        return this.UserToInternal.TryGetValue(userId, out internalId);
    }

    /// <summary>
    /// Returns the user-defined identifier associated with the given internal <see cref="NodeId"/>.
    /// </summary>
    /// <param name="internalId">
    /// The internal identifier to look up.
    /// </param>
    /// <returns>
    /// The corresponding user-defined identifier.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="internalId"/> does not correspond to any registered identifier.
    /// </exception>
    public TUserId GetUserId(NodeId internalId)
    {
        if (this.InternalToUser.TryGetValue(internalId.Value, out TUserId? userId))
        {
            return userId;
        }

        string msg = $"There is no user-front id registered for {internalId.Value}.";
        this.Logger?.LogError(msg);
        throw new ArgumentException(msg, nameof(internalId));
    }

    /// <summary>
    /// Attempts to retrieve the caller-defined identifier associated with the given internal <see cref="NodeId"/>
    /// without throwing if it is not found.
    /// </summary>
    /// <param name="internalId">
    /// The internal identifier to look up.
    /// </param>
    /// <param name="userId">
    /// The corresponding user-defined identifier.
    /// </param>
    /// <returns>
    /// true if <paramref name="internalId"/> is registered; otherwise false.
    /// </returns>
    public bool TryGetUserId(NodeId internalId, [NotNullWhen(true)] out TUserId? userId)
    {
        return this.InternalToUser.TryGetValue(internalId.Value, out userId);
    }

    /// <summary>
    /// Registers <paramref name="userId"/> and assigns it a new internal <see cref="NodeId"/>.
    /// </summary>
    /// <param name="userId">
    /// The user-defined identifier to register. Must not already be registered.
    /// </param>
    /// <returns>
    /// The newly assigned <see cref="NodeId"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="userId"/> has already been registered in this instance.
    /// </exception>
    public NodeId Register(TUserId userId)
    {
        if (!this.UserToInternal.ContainsKey(userId))
        {
            return this.RegisterInternal(userId);
        }

        string msg = $"User ID \"{userId}\" is already registered.";
        this.Logger?.LogError(msg);
        throw new ArgumentException(msg, nameof(userId));
    }

    /// <summary>
    /// Returns the existing internal <see cref="NodeId"/> for <paramref name="userId"/> if it has already been
    /// registered, or registers it and returns a new one.
    /// </summary>
    /// <param name="userId">
    /// The user-defined identifier to look up or register.
    /// </param>
    /// <returns>
    /// The existing or newly assigned <see cref="NodeId"/> for <paramref name="userId"/>.
    /// </returns>
    public NodeId GetOrRegister(TUserId userId)
    {
        if (this.UserToInternal.TryGetValue(userId, out var existing))
        {
            this.Logger?.LogDebug($"Returning existing registration \"{existing.Value}\" for \"{userId}\" (registered by another thread).");
            return existing;
        }

        var newId = this.RegisterInternal(userId);
        this.Logger?.LogDebug($"Registered new user ID \"{userId}\" as \"{newId.Value}\".");
        return newId;
    }

    /// <summary>
    /// Creates and records the bidirectional mapping for <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">
    /// The identifier to register.
    /// </param>
    /// <returns>
    /// The newly assigned <see cref="NodeId"/>.
    /// </returns>
    private NodeId RegisterInternal(TUserId userId)
    {
        var internalId = new NodeId(this.nextId++);
        this.UserToInternal[userId] = internalId;
        this.InternalToUser[internalId.Value] = userId;
        this.Logger?.LogDebug($"Registering new user id \"{userId}\" as \"{internalId.Value}\".");
        return internalId;
    }
}
