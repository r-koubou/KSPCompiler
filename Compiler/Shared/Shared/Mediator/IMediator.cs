namespace KSPCompiler.Shared.Mediator;

public interface IMediator<in TRequest, out TResponse>
{
    TResponse Request( TRequest request );
}
