using System;

using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Compilation.Domain.Messages;

/// <summary>
/// Represents the message data per case.
/// </summary>
public class CompilationMessage
{
    /// <summary>
    /// Datetime of message creation
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public DateTime DateTime { get; }

    /// <summary>
    /// Message level
    /// </summary>
    public CompilationMessageLevel Level { get; }

    /// <summary>
    /// Message body
    /// </summary>
    public CompilationMessageText Text { get; }

    /// <summary>
    /// Text location information to which this message applies.
    /// </summary>
    public Position Position { get; }

    /// <summary>
    /// If the message contains an exception, a non-null exception is stored.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public Exception? Exception { get; }

    /// <summary>
    /// true if <see cref="Exception"/> is not null otherwise false.
    /// </summary>
    public bool HasException
        => Exception is not null;

    /// <summary>
    /// Ctor
    /// </summary>
    public CompilationMessage( CompilationMessageLevel level, CompilationMessageText text, Position position, Exception? exception = null )
    {
        DateTime  = DateTime.Now;
        Level     = level;
        Text      = text;
        Position  = position;
        Exception = exception;
    }
}
