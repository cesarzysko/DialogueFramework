// <copyright file="ConsoleLogger.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework.Tests.XUnit;

using Xunit.Abstractions;

/// <summary>
/// Simple console logger for debugging.
/// </summary>
internal sealed class ConsoleLogger(ITestOutputHelper testOutputHelper) : ILogger
{
    /// <inheritdoc/>
    public void LogDebug(string msg)
    {
        testOutputHelper.WriteLine($"[{DateTime.Now}][INFO] {msg}");
    }

    /// <inheritdoc/>
    public void LogWarning(string msg)
    {
        testOutputHelper.WriteLine($"[{DateTime.Now}][WARN] {msg}");
    }

    /// <inheritdoc/>
    public void LogError(string msg)
    {
        testOutputHelper.WriteLine($"[{DateTime.Now}][ERROR] {msg}");
    }
}
