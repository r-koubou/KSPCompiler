using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Compilation.Gateways.EventEmitting;

public interface ILogEvent : IEvent
{
    string Message { get; }
    Position ScriptPosition { get; }
    string CallerFilePath { get; }
    int CallerLineNumber { get; }
}
