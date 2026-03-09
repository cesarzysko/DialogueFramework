// <copyright file="TargetResolver.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// A default implementation of <see cref="ITargetResolver"/>.
/// </summary>
internal class TargetResolver : ITargetResolver
{
    private readonly IReadOnlyList<TargetEntry> targets;
    private readonly ILogger? logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TargetResolver"/> class.
    /// </summary>
    /// <param name="targets">
    /// All possible target nodes of a choice using this resolver.
    /// </param>
    /// <param name="logger">
    /// The optional logger to inform of internal messages.
    /// </param>
    public TargetResolver(IReadOnlyList<TargetEntry>? targets = null, ILogger? logger = null)
    {
        this.targets = targets ?? [new TargetEntry(null, null)]; // Defaults to unconditional terminal choice.
        this.logger = logger;
    }

    /// <inheritdoc/>
    public NodeId? Resolve(IReadOnlyValueRegistry? registry)
    {
        IReadOnlyList<NodeId?> validEntries = this.targets
            .Where(e => e.Condition?.Evaluate(registry) ?? true)
            .Select(e => e.Target)
            .ToList();

        switch (validEntries.Count)
        {
            case 0:
            {
                const string msg = "None of the target conditions were evaluated as true.";
                this.logger?.LogError(msg);
                throw new InvalidOperationException(msg);
            }

            case >= 2:
            {
                const string msg = "Two or more target conditions were evaluated as true. First target will be selected.";
                this.logger?.LogWarning(msg);
                break;
            }
        }

        return validEntries[0];
    }

    /// <inheritdoc/>
    public IReadOnlyList<NodeId?> GetAllTargets()
    {
        return this.targets.Select(e => e.Target).ToList();
    }
}
