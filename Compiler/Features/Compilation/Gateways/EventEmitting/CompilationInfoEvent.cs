using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Compilation.Gateways.EventEmitting;

public readonly struct CompilationInfoEvent : IEvent
{
    public string Message { get; }

    public Position Position { get; }

    public CompilationInfoEvent( string message, int line = 0, int column = 0 )
    {
        Message = message;
        Position = new Position
        {
            BeginLine   = line,
            EndLine     = line,
            BeginColumn = column,
            EndColumn   = column
        };
    }

    public CompilationInfoEvent( string message, Position position )
    {
        Message  = message;
        Position = position;
    }
}
