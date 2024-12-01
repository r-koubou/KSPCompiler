namespace KSPCompiler.Domain.Events;

public readonly struct CompilationFatalEvent( string message, int line = 0, int column = 0 ) : IEvent
{
    public string Message { get; } = message;
    public int Line { get; } = line;
    public int Column { get; } = column;
}
