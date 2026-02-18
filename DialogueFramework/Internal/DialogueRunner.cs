// <copyright file="DialogueRunner.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Defines a common type used for dialogue nodes traversal.
/// </summary>
/// <param name="graph">The dialogue graph to traverse.</param>
/// <param name="variables">The variables source for condition evaluation and action execution.</param>
/// <param name="startNode">The dialogue node at which the runner should begin.</param>
/// <typeparam name="TDialogueContent">The type of the content which the dialogue node holds and displays.</typeparam>
/// <typeparam name="TChoiceContent">The type of the content which the dialogue choice holds and displays.</typeparam>
internal sealed class DialogueRunner<TDialogueContent, TChoiceContent>(
    DialogueGraph<TDialogueContent, TChoiceContent> graph,
    IVariableStore? variables,
    NodeId startNode)
    : IDialogueRunner<TDialogueContent, TChoiceContent>
{
    private bool isCompleted;

    /// <inheritdoc/>
    public IDialogueNode<TDialogueContent, TChoiceContent> Current { get; private set; } =
        graph.GetDialogueNode(startNode);

    private DialogueGraph<TDialogueContent, TChoiceContent> Graph { get; } = graph;

    private IVariableStore? Variables { get; } = variables;

    private NodeId StartNode { get; } = startNode;

    /// <inheritdoc/>
    public void Reset()
    {
        this.Current = this.Graph.GetDialogueNode(this.StartNode);
    }

    /// <inheritdoc/>
    public IReadOnlyList<IDialogueChoice<TChoiceContent>> GetAvailableChoices()
    {
        return this.Current.Choices
            .Where(c => c.Condition?.Evaluate(this.Variables) ?? true)
            .ToArray();
    }

    /// <inheritdoc/>
    public IReadOnlyList<IDialogueChoice<TChoiceContent>> GetChoices()
    {
        return this.Current.Choices.ToArray();
    }

    /// <inheritdoc/>
    public bool Choose(IDialogueChoice<TChoiceContent> choice)
    {
        if (this.isCompleted)
        {
            return false;
        }

        ArgumentNullException.ThrowIfNull(choice);
        if (!this.Current.Choices.Contains(choice))
        {
            throw new ArgumentException("Choice not from current node.", nameof(choice));
        }

        choice.Action?.Execute(this.Variables);

        if (choice.Target == null)
        {
            this.isCompleted = true;
            return false;
        }

        try
        {
            this.Current = this.Graph.GetDialogueNode(choice.Target.Value);
            return true;
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidOperationException($"Target node {choice.Target.Value.Value} not found in graph.", ex);
        }
    }

    /// <inheritdoc/>
    public bool IsCompleted()
    {
        return this.isCompleted;
    }
}
