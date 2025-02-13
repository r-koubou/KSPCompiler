using KSPCompiler.Commons.Text;

namespace KSPCompiler.Gateways.EventEmitting;

public readonly struct CompilationFatalEvent : IEvent
{
    public string Message { get; }

    public Position Position { get; }

    public CompilationFatalEvent( string message, int line = 0, int column = 0 )
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

    public CompilationFatalEvent( string message, Position position )
    {
        Message  = message;
        Position = position;
    }
}
