// <copyright file="IDialogueRunner.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Controls the traversal of a dialogue graph, exposing the current node and advancing state in response to user
/// choices.
/// </summary>
/// <typeparam name="TRegistryKey">
/// The key type used to identify values in the <see cref="IValueRegistry{TKey}"/>.
/// </typeparam>
/// <typeparam name="TDialogueContent">
/// The type of displayable data carried by each dialogue node.
/// </typeparam>
/// <typeparam name="TChoiceContent">
/// The type of displayable data carried by each dialogue choice.
/// </typeparam>
public interface IDialogueRunner<TRegistryKey, out TDialogueContent, TChoiceContent>
    where TRegistryKey : notnull
{
    /// <summary>
    /// Gets the node the runner is currently positioned on.
    /// </summary>
    public IDialogueNode<TRegistryKey, TDialogueContent, TChoiceContent>? Current { get; }

    /// <summary>
    /// Returns the choices of the current node that satisfy their associated conditions, and are therefore eligible
    /// for the player to select.
    /// </summary>
    /// <returns>
    /// A snapshot of the choices currently available.
    /// </returns>
    public IReadOnlyList<IDialogueChoice<TRegistryKey, TChoiceContent>> GetAvailableChoices();

    /// <summary>
    /// Returns all choices of the current node, regardless of whether their conditions are satisfied.
    /// </summary>
    /// <returns>
    /// A snapshot of every choice defined on the current node, regardless of whether their conditions are satisfied.
    /// </returns>
    public IReadOnlyList<IDialogueChoice<TRegistryKey, TChoiceContent>> GetChoices();

    /// <summary>
    /// Executes the action associated with <paramref name="choice"/> and, if the choice leads to another
    /// node, advances <see cref="Current"/> to that node.
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
    /// Thrown when <paramref name="choice"/> does not belong to the current node.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the choice's target node is not found in the dialogue graph.
    /// </exception>
    public bool Choose(IDialogueChoice<TRegistryKey, TChoiceContent> choice);

    /// <summary>
    /// Returns whether the dialogue has reached a terminal choice and can no longer advance.
    /// </summary>
    /// <returns>
    /// Whether the dialogue reached a terminal node or not.
    /// </returns>
    public bool ReachedTerminalNode();

    /// <summary>
    /// Resets the runner to its initial state, positioning <see cref="Current"/> back to the start node and allowing
    /// the dialogue to be traversed again.
    /// </summary>
    public void Reset();
}
