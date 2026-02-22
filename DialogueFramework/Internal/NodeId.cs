// <copyright file="NodeId.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// A lightweight integer identifier assigned to a dialogue node by the framework at build time.
/// </summary>
internal readonly record struct NodeId(int Value);
