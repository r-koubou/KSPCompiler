using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Compilation.Gateways.EventEmitting;

public readonly struct CompilationWarningEvent : ICompilationEvent
{
    public string Message { get; }

    public Position Position { get; }

    public CompilationWarningEvent( string message, int line = 0, int column = 0 )
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

    public CompilationWarningEvent( string message, Position position )
    {
        Message  = message;
        Position = position;
    }
}
