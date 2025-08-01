namespace KSPCompiler.Shared.EventEmitting;

public class TextMessageEvent( string message ) : IEvent<string>
{
    public string Message { get; } = message;

    public override string ToString()
        => Message;
}
