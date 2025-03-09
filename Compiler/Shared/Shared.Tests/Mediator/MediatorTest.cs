using KSPCompiler.Shared.Mediator;

using NUnit.Framework;

namespace KSPCompiler.Shared.Tests.Mediator;

[TestFixture]
public class MediatorTest
{
    [Test]
    public void SendAndReceiveExpectedTest()
    {
        var mediator = new MockMediator
        {
            RequestHandler = new MockRequestHandler()
        };

        var response = mediator.Request( 123 );

        Assert.That( response, Is.EqualTo( "123" ) );
    }

    private class MockMediator : IMediator<int, string>
    {
        public IRequestHandler<int, string> RequestHandler { get; set; } = null!;

        public string Request( int request )
        {
            return RequestHandler.Handle( request );
        }
    }

    private class MockRequestHandler : IRequestHandler<int, string>
    {
        public string Handle( int request )
        {
            return request.ToString();
        }
    }
}
