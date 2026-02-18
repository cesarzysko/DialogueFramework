// <copyright file="NodeIdRegistry.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Defines a common type for registering user-front ids as internal ids.
/// </summary>
/// <param name="logger">An optional logger to write internal messages to.</param>
/// <typeparam name="TUserId">The type of the user-front id.</typeparam>
internal sealed class NodeIdRegistry<TUserId>(
    ILogger? logger = null)
        where TUserId : notnull
{
    private readonly object lockObj = new();
    private int nextId;

    private Dictionary<TUserId, NodeId> UserToInternal { get; } = new();

    private Dictionary<int, TUserId> InternalToUser { get; } = new();

    private ILogger? Logger { get; } = logger;

    /// <summary>
    /// Returns the internal id associated with the given user-front id.
    /// </summary>
    /// <param name="userId">The user-front id associated with the internal id to be returned.</param>
    /// <returns>The internal id associated with the given user-front id.</returns>
    /// <exception cref="ArgumentException">When the user-front id does not correspond to any internal id.</exception>
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
    /// Returns the internal id if it is associated with the given user-front id.
    /// </summary>
    /// <param name="userId">The user-front id associated with the internal id to be returned.</param>
    /// <param name="internalId">The returned internal id associated with the given user-front id.</param>
    /// <returns>Whether the internal id was successfully retrieved from the registry.</returns>
    public bool TryGetInternalId(TUserId userId, out NodeId internalId)
    {
        return this.UserToInternal.TryGetValue(userId, out internalId);
    }

    /// <summary>
    /// Return the user-front id associated with the given internal id.
    /// </summary>
    /// <param name="internalId">The internal id associated with the user-front id to be returned.</param>
    /// <returns>The user-front id associated with the given internal id.</returns>
    /// <exception cref="ArgumentException">When the internal id does not correspond to any user-front id.</exception>
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
    /// Returns the user-front id if it is associated with the given internal id.
    /// </summary>
    /// <param name="internalId">The internal id associated with the user-front id to be returned.</param>
    /// <param name="userId">The user-front id associated with the given internal id.</param>
    /// <returns>Whether the user-front id was successfully retrieved from the registry.</returns>
    public bool TryGetUserId(NodeId internalId, [NotNullWhen(true)] out TUserId? userId)
    {
        return this.InternalToUser.TryGetValue(internalId.Value, out userId);
    }

    /// <summary>
    /// Registers the user-front id as an internal id and returns it.
    /// </summary>
    /// <param name="userId">The user-front id to register.</param>
    /// <returns>The newly created internal id corresponding to the registered user-front id.</returns>
    /// <exception cref="ArgumentException">If the user id is already registered.</exception>
    public NodeId Register(TUserId userId)
    {
        lock (this.lockObj)
        {
            if (!this.UserToInternal.ContainsKey(userId))
            {
                return this.RegisterInternal(userId);
            }

            string msg = $"User ID \"{userId}\" is already registered.";
            this.Logger?.LogError(msg);
            throw new ArgumentException(msg, nameof(userId));
        }
    }

    /// <summary>
    /// Registers the user-front id as an internal id and returns it, or an existing one if already registered.
    /// </summary>
    /// <param name="userId">The user-front id to register.</param>
    /// <returns>The newly created or existing internal id corresponding to the registered user-front id.</returns>
    public NodeId GetOrRegister(TUserId userId)
    {
        if (this.UserToInternal.TryGetValue(userId, out var existing))
        {
            this.Logger?.LogDebug($"Returning existing registration \"{existing.Value}\" for \"{userId}\".");
            return existing;
        }

        lock (this.lockObj)
        {
            if (this.UserToInternal.TryGetValue(userId, out existing))
            {
                this.Logger?.LogDebug($"Returning existing registration \"{existing.Value}\" for \"{userId}\" (registered by another thread).");
                return existing;
            }

            var newId = this.RegisterInternal(userId);
            this.Logger?.LogDebug($"Registered new user ID \"{userId}\" as \"{newId.Value}\".");
            return newId;
        }
    }

    private NodeId RegisterInternal(TUserId userId)
    {
        var internalId = new NodeId(this.nextId++);
        this.UserToInternal[userId] = internalId;
        this.InternalToUser[internalId.Value] = userId;
        this.Logger?.LogDebug($"Registering new user id \"{userId}\" as \"{internalId.Value}\".");
        return internalId;
    }
}
