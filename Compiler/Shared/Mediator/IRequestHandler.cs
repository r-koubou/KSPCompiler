namespace KSPCompiler.Shared.Mediator;

public interface IRequestHandler<in TRequest, out TResponse>
{
    TResponse Handle( TRequest request );
}
