// <copyright file="GlobalSuppressions.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "SonarAnalyzer.CSharp",
    "SA1602",
    Justification = "Enumerations are self-explaining.",
    Scope = "type",
    Target = "~T:DialogueFramework.Tests.XUnit.SceneId")]
