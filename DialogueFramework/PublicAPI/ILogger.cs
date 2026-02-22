// <copyright file="ILogger.cs" company="SPS">
// Copyright (c) SPS. All rights reserved.
// </copyright>

namespace DialogueFramework;

/// <summary>
/// Exposes logging methods for receiving internal diagnostic messages from the framework.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Logs a message intended for development diagnostics.
    /// </summary>
    /// <param name="msg">
    /// The diagnostic message to log.
    /// </param>
    void LogDebug(string msg);

    /// <summary>
    /// Logs a message indicating a recoverable issue that may lead to unexpected behavior.
    /// </summary>
    /// <param name="msg">
    /// The warning message to log.
    /// </param>
    void LogWarning(string msg);

    /// <summary>
    /// Logs a message indicating a non-recoverable failure within the framework.
    /// </summary>
    /// <param name="msg">
    /// The error message to log.
    /// </param>
    void LogError(string msg);
}
