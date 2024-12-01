using KSPCompiler.Domain.Events;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Events;

[TestFixture]
public class EventTest
{
    [Test]
    public void DispatchTest()
    {
        var dispatcher = new EventDispatcher();
        var observer   = new MockIntEventObserver();
        using var token = dispatcher.Subscribe( observer );

        dispatcher.Dispatch( new MockIntEvent( 1 ) );
        dispatcher.Dispatch( new MockIntEvent( 2 ) );
        dispatcher.Dispatch( new MockIntEvent( 3 ) );

        Assert.That( observer.total, Is.EqualTo( 6 ) );
    }
}
