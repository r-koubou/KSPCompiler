using System.Threading;
using System.Threading.Tasks;

namespace KSPCompiler.Shared.Mediator;

public interface IMediator<in TRequest, TResponse>
{
    TResponse Request( TRequest request )
        => RequestAsync( request ).GetAwaiter().GetResult();

    Task<TResponse> RequestAsync( TRequest request, CancellationToken token = default );
}
