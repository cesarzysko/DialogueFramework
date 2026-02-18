// <copyright file="ILogger.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// An interface to be used to communicate certain internal messages.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Writes a debug log message.
    /// </summary>
    /// <param name="msg">The message to write.</param>
    void LogDebug(string msg);

    /// <summary>
    /// Writes a warning log message.
    /// </summary>
    /// <param name="msg">The message to write.</param>
    void LogWarning(string msg);

    /// <summary>
    /// Writes an error log message.
    /// </summary>
    /// <param name="msg">The message to write.</param>
    void LogError(string msg);
}
