using KSPCompiler.Applications.Commons.Events;

using NUnit.Framework;

namespace KSPCompiler.Applications.Commons.Tests.Events;

[TestFixture]
public class EventTest
{
    [Test]
    public void DispatchTest()
    {
        var dispatcher = new EventEmitter();
        var observer   = new MockIntEventObserver();
        using var token = dispatcher.Subscribe( observer );

        dispatcher.Dispatch( new MockIntEvent( 1 ) );
        dispatcher.Dispatch( new MockIntEvent( 2 ) );
        dispatcher.Dispatch( new MockIntEvent( 3 ) );

        Assert.That( observer.total, Is.EqualTo( 6 ) );
    }
}
