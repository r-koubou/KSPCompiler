using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Mediator;

using NUnit.Framework;

namespace KSPCompiler.Shared.Tests.Mediator;

[TestFixture]
public class MediatorTest
{
    [Test]
    public async Task SendAndReceiveExpectedTest()
    {
        var mediator = new MockMediator
        {
            RequestHandler = new MockRequestHandler()
        };

        var response = await mediator.RequestAsync( 123, CancellationToken.None );

        Assert.That( response, Is.EqualTo( "123" ) );
    }

    private class MockMediator : IMediator<int, string>
    {
        public IRequestHandler<int, string> RequestHandler { get; set; } = null!;

        public Task<string> RequestAsync( int request, CancellationToken token = default )
        {
            return Task.FromResult( RequestHandler.Handle( request ) );
        }
    }

    private class MockRequestHandler : IRequestHandler<int, string>
    {
        public string Handle( int request )
        {
            return request.ToString();
        }

        public Task<string> HandleAsync( int request, CancellationToken token = default )
        {
            return Task.FromResult( Handle( request ) );
        }
    }
}
