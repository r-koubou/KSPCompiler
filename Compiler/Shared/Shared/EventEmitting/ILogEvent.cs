using KSPCompiler.Shared.Text;

namespace KSPCompiler.Shared.EventEmitting;

public interface ILogEvent : IEvent
{
    string Message { get; }
    Position ScriptPosition { get; }
    string CallerFilePath { get; }
    int CallerLineNumber { get; }
}
