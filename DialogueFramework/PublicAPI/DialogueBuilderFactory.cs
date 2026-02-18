// <copyright file="DialogueBuilderFactory.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// A static class used to create a dialogue builder.
/// </summary>
public static class DialogueBuilderFactory
{
    /// <summary>
    /// Creates a dialogue builder.
    /// </summary>
    /// <param name="logger">The optional logger to use to print internal messages.</param>
    /// <typeparam name="TUserId">The user-front dialogues identification type.</typeparam>
    /// <typeparam name="TDialogueContent">The displayable content of dialogues.</typeparam>
    /// <typeparam name="TChoiceContent">The displayable content of dialogue choices.</typeparam>
    /// <returns>The created dialogue builder.</returns>
    public static IDialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent> CreateBuilder<TUserId, TDialogueContent, TChoiceContent>(
        ILogger? logger = null)
        where TUserId : notnull
    {
        return new DialogueNodeBuilder<TUserId, TDialogueContent, TChoiceContent>(logger);
    }
}
