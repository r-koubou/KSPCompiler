using System.Runtime.CompilerServices;

using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Compilation.Gateways.EventEmitting;

public readonly struct LogErrorEvent : ILogEvent
{
    public string Message { get; }
    public Position ScriptPosition { get; }

    public string CallerFilePath { get; }
    public int CallerLineNumber { get; }

    public LogErrorEvent(
        string message,
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0 )
    {
        Message          = message;
        ScriptPosition   = Position.Zero;
        CallerFilePath   = callerFilePath;
        CallerLineNumber = callerLineNumber;
    }

    public LogErrorEvent(
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
