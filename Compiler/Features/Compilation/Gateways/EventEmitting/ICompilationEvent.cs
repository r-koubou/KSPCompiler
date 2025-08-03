using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Compilation.Gateways.EventEmitting;

public interface ICompilationEvent : IEvent
{
    public string Message { get; }
    public Position Position { get; }
}
