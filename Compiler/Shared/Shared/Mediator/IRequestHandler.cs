using System.Threading;
using System.Threading.Tasks;

namespace KSPCompiler.Shared.Mediator;

public interface IRequestHandler<in TRequest, TResponse>
{
    TResponse Handle( TRequest request )
        => HandleAsync( request ).GetAwaiter().GetResult();

    Task<TResponse> HandleAsync( TRequest request, CancellationToken token = default );
}
