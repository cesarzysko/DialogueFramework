// <copyright file="RunnerFacade.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Controls the traversal of a dialogue graph, exposing the current node and advancing state in response to user
/// choices.
/// </summary>
/// <typeparam name="TDialogueContent">
/// The type of displayable data carried by each dialogue node.
/// </typeparam>
/// <typeparam name="TChoiceContent">
/// The type of displayable data carried by each dialogue choice.
/// </typeparam>
public sealed class RunnerFacade<TDialogueContent, TChoiceContent>
{
    private readonly IRunner<TDialogueContent, TChoiceContent> runner;

    /// <summary>
    /// Initializes a new instance of the <see cref="RunnerFacade{TDialogueContent, TChoiceContent}"/> class.
    /// </summary>
    /// <param name="runner">
    /// The internal runner implementation which this class hides and on which it depends.
    /// </param>
    internal RunnerFacade(IRunner<TDialogueContent, TChoiceContent> runner)
    {
        this.runner = runner;
    }

    /// <summary>
    /// Gets the node content the runner is currently positioned on.
    /// </summary>
    public TDialogueContent CurrentContent
        => this.runner.Current!.Content;

    /// <summary>
    /// Returns the choices of the current node that satisfy their associated conditions, and are therefore eligible
    /// for the player to select.
    /// </summary>
    /// <returns>
    /// A snapshot of the choices currently available.
    /// </returns>
    public IReadOnlyList<AvailableChoiceFacade<TChoiceContent>> GetAvailableChoices()
        => this.runner.GetAvailableChoices().Select(ch => new AvailableChoiceFacade<TChoiceContent>(ch)).ToList();

    /// <summary>
    /// Returns all choices of the current node, regardless of whether their conditions are satisfied.
    /// </summary>
    /// <returns>
    /// A snapshot of every choice defined on the current node, regardless of whether their conditions are satisfied.
    /// </returns>
    public IReadOnlyList<ChoiceFacade<TChoiceContent>> GetChoices()
        => this.runner.GetChoices().Select(ch => new ChoiceFacade<TChoiceContent>(ch)).ToList();

    /// <summary>
    /// Executes the action associated with <paramref name="choice"/> and, if the choice leads to another
    /// node, advances <see cref="CurrentContent"/> to that node.
    /// </summary>
    /// <param name="choice">
    /// A choice obtained from <see cref="GetAvailableChoices"/>.
    /// </param>
    /// <returns>
    /// true if the dialogue advanced to a new node and can continue;
    /// false if the selected choice had no target, ending the dialogue.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="choice"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="choice"/> does not belong to the current node;
    /// or when the specified condition is not met.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the choice's target node is not found in the dialogue graph;
    /// or when the runner already reached a terminal node.
    /// </exception>
    public bool Choose(AvailableChoiceFacade<TChoiceContent> choice)
        => this.runner.Choose(choice.Choice);

    /// <summary>
    /// Returns whether the dialogue has reached a terminal choice and can no longer advance.
    /// </summary>
    /// <returns>
    /// Whether the dialogue reached a terminal node or not.
    /// </returns>
    public bool ReachedTerminalNode()
        => this.runner.ReachedTerminalNode();

    /// <summary>
    /// Resets the runner to its initial state, positioning <see cref="CurrentContent"/> back to the start node and allowing
    /// the dialogue to be traversed again.
    /// </summary>
    public void Reset()
        => this.runner.Reset();
}
