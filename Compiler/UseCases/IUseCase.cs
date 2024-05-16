using System.Threading;
using System.Threading.Tasks;

namespace KSPCompiler.UseCases;

public interface IUseCase
{
    void Execute()
        => ExecuteAsync().GetAwaiter().GetResult();

    Task ExecuteAsync( CancellationToken cancellationToken = default );
}

public interface IUseCase<in TInputPort, TOutputPort>
    where TInputPort : IInputPort
    where TOutputPort : IOutputPort
{
    TOutputPort Execute( TInputPort input )
        => ExecuteAsync( input ).GetAwaiter().GetResult();

    Task<TOutputPort> ExecuteAsync( TInputPort input, CancellationToken cancellationToken = default );
}
