// <copyright file="CompositeCondition.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An <see cref="ICondition"/> that checks a fixed sequence of other conditions in order.
/// </summary>
public class CompositeCondition : ICondition
{
    private readonly ICondition[] conditions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeCondition"/> class.
    /// </summary>
    /// <param name="conditions">
    /// The conditions to check in order when this composite condition is checked.
    /// </param>
    public CompositeCondition(params ICondition[] conditions)
    {
        this.conditions = conditions;
    }

    /// <inheritdoc/>
    public bool Evaluate(IReadOnlyValueRegistry? valueRegistry)
    {
        return this.conditions.All(c => c.Evaluate(valueRegistry));
    }
}
