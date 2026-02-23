// <copyright file="Runner.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// The default implementation of <see cref="IRunner{TDialogueContent,TChoiceContent}"/> that
/// traverses a <see cref="Graph{TDialogueContent,TChoiceContent}"/> in response to player choices.
/// </summary>
/// <typeparam name="TRegistryKey">
/// The key type used to identify values in the <see cref="IValueRegistry{TKey}"/>.
/// </typeparam>
/// <typeparam name="TDialogueContent">
/// The type of displayable data carried by each dialogue node.
/// </typeparam>
/// <typeparam name="TChoiceContent">
/// The type of displayable data carried by each choice.
/// </typeparam>
internal sealed class Runner<TRegistryKey, TDialogueContent, TChoiceContent>
    : IRunner<TDialogueContent, TChoiceContent>
    where TRegistryKey : notnull
{
    private readonly Graph<TDialogueContent, TChoiceContent> graph;
    private readonly IValueRegistry<TRegistryKey>? valueRegistry;
    private readonly NodeId startNode;
    private bool reachedTerminalNode;

    /// <summary>
    /// Initializes a new instance of the <see cref="Runner{TRegistryKey,TDialogueContent,TChoiceContent}"/> class.
    /// </summary>
    /// <param name="graph">
    /// The dialogue graph to traverse.
    /// </param>
    /// <param name="valueRegistry">
    /// The registry made available to conditions and actions.
    /// </param>
    /// <param name="startNode">
    /// The internal identifier of the node at which traversal should begin.
    /// </param>
    internal Runner(
        Graph<TDialogueContent, TChoiceContent> graph,
        IValueRegistry<TRegistryKey>? valueRegistry,
        NodeId startNode)
    {
        this.Current = graph.GetDialogueNode(startNode);
        this.graph = graph;
        this.valueRegistry = valueRegistry;
        this.startNode = startNode;
    }

    /// <inheritdoc/>
    public INode<TDialogueContent, TChoiceContent>? Current { get; private set; }

    /// <inheritdoc/>
    public void Reset()
    {
        this.reachedTerminalNode = false;
        this.Current = this.graph.GetDialogueNode(this.startNode);
    }

    /// <inheritdoc/>
    public IReadOnlyList<IChoice<TChoiceContent>> GetAvailableChoices()
    {
        if (this.Current == null)
        {
            return [];
        }

        return this.Current.Choices
            .Where(c => c.Condition?.Evaluate(this.valueRegistry) ?? true)
            .ToArray();
    }

    /// <inheritdoc/>
    public IReadOnlyList<IChoice<TChoiceContent>> GetChoices()
    {
        if (this.Current == null)
        {
            return [];
        }

        return this.Current.Choices.ToArray();
    }

    /// <inheritdoc/>
    public bool Choose(IChoice<TChoiceContent> choice)
    {
        if (this.reachedTerminalNode)
        {
            throw new InvalidOperationException("The runner has already reached a terminal node.");
        }

        ArgumentNullException.ThrowIfNull(choice);
        if (this.Current == null || !this.Current.Choices.Contains(choice))
        {
            throw new ArgumentException("Choice is not from the current node.", nameof(choice));
        }

        if (!choice.Condition?.Evaluate(this.valueRegistry) ?? false)
        {
            throw new ArgumentException("Choice does not meet the condition specified.", nameof(choice));
        }

        choice.Action?.Execute(this.valueRegistry);

        if (choice.Target == null)
        {
            this.Current = null;
            this.reachedTerminalNode = true;
            return false;
        }

        try
        {
            this.Current = this.graph.GetDialogueNode(choice.Target.Value);
            return true;
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidOperationException($"Target node {choice.Target.Value.Value} not found in graph.", ex);
        }
    }

    /// <inheritdoc/>
    public bool ReachedTerminalNode()
    {
        return this.reachedTerminalNode;
    }
}
