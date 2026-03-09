// <copyright file="TargetEntry.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Encapsulates a choice routed target which is achieved when a condition is met.
/// Ideally all conditions must be mutually exclusive with only one target always achievable.
/// </summary>
/// <param name="Condition">
/// The condition that must be met in order for the target to be valid.
/// Null ONLY for fallback targets if all other conditions are evaluated to be false.
/// </param>
/// <param name="Target">
/// The target node of a choice if the condition is evaluated to be true.
/// Null ONLY if under the conditions met the choice is meant to terminate the dialogue.
/// </param>
internal record TargetEntry(ICondition? Condition, NodeId? Target);
