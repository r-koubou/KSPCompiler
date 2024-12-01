using KSPCompiler.Domain.Events;

namespace KSPCompiler.Domain.Tests.Events;

public class MockIntEvent( int value ) : IEvent
{
    public int Value { get; } = value;
}
