using System;

namespace KSPCompiler.Domain.Tests.Events;

public class MockIntEventObserver : IObserver<MockIntEvent>
{
    public int total = 0;
    public void OnNext( MockIntEvent value )
    {
        total += value.Value;
    }
    public void OnCompleted() {}
    public void OnError( Exception error ) {}
}
