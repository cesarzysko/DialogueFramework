// <copyright file="CompositeCondition.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An <see cref="ICondition{TRegistryKey}"/> that checks a fixed sequence of other conditions in order.
/// </summary>
/// <typeparam name="TRegistryKey">
/// The key type used to identify values in the <see cref="IValueRegistry{TKey}"/>.
/// </typeparam>
public class CompositeCondition<TRegistryKey> : ICondition<TRegistryKey>
    where TRegistryKey : notnull
{
    private readonly ICondition<TRegistryKey>[] conditions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeCondition{TRegistryKey}"/> class.
    /// </summary>
    /// <param name="conditions">
    /// The conditions to check in order when this composite condition is checked.
    /// </param>
    public CompositeCondition(params ICondition<TRegistryKey>[] conditions)
    {
        this.conditions = conditions;
    }

    /// <inheritdoc/>
    public bool Evaluate(IValueRegistry<TRegistryKey>? valueRegistry)
    {
        return this.conditions.All(c => c.Evaluate(valueRegistry));
    }
}
