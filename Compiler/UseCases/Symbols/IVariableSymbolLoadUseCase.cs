using System;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class VariableSymbolLoadOutputData : IOutputPort<ISymbolTable<VariableSymbol>>
{
    public bool Result { get; }
    public ISymbolTable<VariableSymbol> OutputData { get; }
    public Exception? Error { get; }

    public VariableSymbolLoadOutputData( bool result, ISymbolTable<VariableSymbol> table, Exception? error = null )
    {
        Result     = result;
        OutputData = table;
        Error      = error;
    }
}

public interface IVariableSymbolLoadUseCase : IUseCase<Unit, VariableSymbolLoadOutputData> {}
