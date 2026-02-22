// <copyright file="GlobalSuppressions.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

// TODO: Update existing and add missing suppressions.
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "SonarAnalyzer.CSharp",
    "S2436",
    Justification = "Three generic parameters are intentional in this builder abstraction.",
    Scope = "type",
    Target = "~T:DialogueFramework.DialogueNodeBuilder`3")]

[assembly: SuppressMessage(
    "SonarAnalyzer.CSharp",
    "S2436",
    Justification = "Three generic parameters are intentional in this builder abstraction.",
    Scope = "type",
    Target = "~T:DialogueFramework.IDialogueNodeBuilder`3")]

[assembly: SuppressMessage(
    "SonarAnalyzer.CSharp",
    "S2436",
    Justification = "Three generic parameters are intentional in this builder abstraction.",
    Scope = "type",
    Target = "~T:DialogueFramework.IAdvancedDialogueNodeBuilder`3")]

[assembly: SuppressMessage(
    "SonarAnalyzer.CSharp",
    "S2436",
    Justification = "Three generic parameters are intentional in this builder abstraction.",
    Scope = "type",
    Target = "~T:DialogueFramework.IDialogueNodeChoiceBuilder`3")]
