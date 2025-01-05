using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Events;

public interface ILogEvent : IEvent
{
    string Message { get; }
    Position ScriptPosition { get; }
    string CallerFilePath { get; }
    int CallerLineNumber { get; }
}
