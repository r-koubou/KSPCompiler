using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class VariableSymbolTableLoadController
{
    private readonly IVariableSymbolLoadUseCase useCase;

    public VariableSymbolTableLoadController( IVariableSymbolLoadUseCase useCase )
    {
        this.useCase = useCase;
    }

    public(bool Reeult, ISymbolTable<VariableSymbol> Table, Exception? Error) Load()
        => LoadAsync().GetAwaiter().GetResult();

    public async Task<(bool Reeult, ISymbolTable<VariableSymbol> Table, Exception? Error)> LoadAsync( CancellationToken cancellationToken = default )
    {
        var result = await useCase.ExecuteAsync(Unit.Default, cancellationToken );

        return ( result.Result, result.OutputData, result.Error );
    }
}
