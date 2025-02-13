using KSPCompiler.Gateways.EventEmitting;

namespace KSPCompiler.Infrastructures.EventEmitting.Default.Tests;

public class MockIntEvent( int value ) : IEvent
{
    public int Value { get; } = value;
}
