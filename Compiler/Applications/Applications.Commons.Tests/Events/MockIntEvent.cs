using KSPCompiler.Domain.Events;

namespace KSPCompiler.Applications.Commons.Tests.Events;

public class MockIntEvent( int value ) : IEvent
{
    public int Value { get; } = value;
}
