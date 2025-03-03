using KSPCompiler.Commons.Text;

namespace KSPCompiler.Gateways.EventEmitting;

public interface ILogEvent : IEvent
{
    string Message { get; }
    Position ScriptPosition { get; }
    string CallerFilePath { get; }
    int CallerLineNumber { get; }
}
