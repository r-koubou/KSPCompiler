using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Events;

public readonly struct CompilationErrorEvent : IEvent
{
    public string Message { get; }

    public Position Position { get; }

    public CompilationErrorEvent( string message, int line = 0, int column = 0 )
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

    public CompilationErrorEvent( string message, Position position )
    {
        Message  = message;
        Position = position;
    }
}
