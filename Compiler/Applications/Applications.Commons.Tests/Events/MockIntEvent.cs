using KSPCompiler.Domain.Events;

namespace KSPCompiler.Applications.Shared.Tests.Events;

public class MockIntEvent( int value ) : IEvent
{
    public int Value { get; } = value;
}
