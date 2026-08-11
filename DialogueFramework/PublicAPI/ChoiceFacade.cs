// <copyright file="ChoiceFacade.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

public sealed class ChoiceFacade<TChoiceContent>
{
    private readonly IChoice<TChoiceContent> choice;

    internal ChoiceFacade(IChoice<TChoiceContent> choice)
    {
        this.choice = choice;
    }

    public TChoiceContent? Content
        => this.choice.Content;
}
