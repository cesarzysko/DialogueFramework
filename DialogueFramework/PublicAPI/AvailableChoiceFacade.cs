// <copyright file="AvailableChoiceFacade.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

public sealed class AvailableChoiceFacade<TChoiceContent>
{
    private readonly IChoice<TChoiceContent> choice;

    internal IChoice<TChoiceContent> Choice => this.choice;

    internal AvailableChoiceFacade(IChoice<TChoiceContent> choice)
    {
        this.choice = choice;
    }

    public TChoiceContent? Content
        => this.choice.Content;
}
