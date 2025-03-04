using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.Gateways.Symbols;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.Compilation.UseCase.ApplicationServices;

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
            throw new InvalidOperationException( "Failed to load built-in symbols.", result.Error );
        }

        builtInSymbolTable = result.OutputData;

        return builtInSymbolTable;
    }
}
