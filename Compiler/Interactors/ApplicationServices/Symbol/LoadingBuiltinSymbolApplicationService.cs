using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactors.ApplicationServices.Symbol;

public sealed class LoadingBuiltinSymbolApplicationService(
    ILoadBuiltinSymbolUseCase loadBuiltinSymbolUseCase,
    AggregateSymbolRepository symbolRepositories
)
{
    private readonly ILoadBuiltinSymbolUseCase loadBuiltinSymbolUseCase = loadBuiltinSymbolUseCase;
    private readonly AggregateSymbolRepository symbolRepositories = symbolRepositories;
    private AggregateSymbolTable? builtInSymbolTable;

    public async Task<AggregateSymbolTable> LoadAsync( CancellationToken cancellationToken = default )
    {
        if( builtInSymbolTable is not null )
        {
            return builtInSymbolTable;
        }

        var input = new LoadBuiltinSymbolInputData( symbolRepositories );
        var result = await loadBuiltinSymbolUseCase.ExecuteAsync( input, cancellationToken );

        if( !result.Result )
        {
            throw new InvalidOperationException( "Failed to load built-in symbols." );
        }

        builtInSymbolTable = result.OutputData;

        return builtInSymbolTable;
    }
}
