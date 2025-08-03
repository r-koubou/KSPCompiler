namespace KSPCompiler.Shared.EventEmitting;

/// <summary>
/// The base interface for configuring events.
/// </summary>
public interface IEvent {}

/// <summary>
/// A simple interface for events that carry a message.
/// </summary>
public interface IEvent<out TMessage> : IEvent
{
    public TMessage Message { get; }
}
