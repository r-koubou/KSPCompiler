namespace KSPCompiler.Shared.EventEmitting;

public interface ILogEvent : IEvent
{
    string Message { get; }
    string CallerFilePath { get; }
    int CallerLineNumber { get; }
}
