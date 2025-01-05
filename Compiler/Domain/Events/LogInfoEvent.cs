using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Events;

public readonly struct LogInfoEvent : IEvent
{
    public string Message { get; }

    public Position Position { get; }

    public LogInfoEvent( string message, int line = 0, int column = 0 )
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

    public LogInfoEvent( string message, Position position )
    {
        Message  = message;
        Position = position;
    }
}
