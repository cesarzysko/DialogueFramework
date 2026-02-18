// <copyright file="NodeId.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Defines a common immutable type for referencing dialogue nodes.
/// </summary>
internal readonly record struct NodeId(int Value)
{
    /// <summary>
    /// Gets a direct identifier to a node.
    /// </summary>
    public int Value { get; } = Value;
}
