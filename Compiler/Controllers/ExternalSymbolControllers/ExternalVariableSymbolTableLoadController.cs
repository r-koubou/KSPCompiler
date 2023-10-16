using System;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.ExternalSymbolControllers;

public class ExternalVariableSymbolTableLoadController
{
    private readonly IVariableSymbolLoadUseCase useCase;

    public ExternalVariableSymbolTableLoadController( IVariableSymbolLoadUseCase useCase )
    {
        this.useCase = useCase;
    }

    public (bool Reeult, ISymbolTable<VariableSymbol> Table, Exception? Error) Load()
    {
        var result = useCase.Execute();

        return ( result.Result, result.Table, result.Error );
    }
}
