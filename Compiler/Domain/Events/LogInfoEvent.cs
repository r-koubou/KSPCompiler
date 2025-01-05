using System.Runtime.CompilerServices;

using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Events;

public readonly struct LogInfoEvent : ILogEvent
{
    public string Message { get; }
    public Position ScriptPosition { get; }

    public string CallerFilePath { get; }
    public int CallerLineNumber { get; }

    public LogInfoEvent(
        string message,
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0 )
    {
        Message          = message;
        ScriptPosition   = Position.Zero;
        CallerFilePath   = callerFilePath;
        CallerLineNumber = callerLineNumber;
    }

    public LogInfoEvent(
        string message,
        Position scriptPosition,
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0 )
    {
        Message          = message;
        ScriptPosition   = scriptPosition;
        CallerFilePath   = callerFilePath;
        CallerLineNumber = callerLineNumber;
    }
}
