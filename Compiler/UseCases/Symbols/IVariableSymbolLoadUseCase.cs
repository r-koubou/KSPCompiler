using System;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class VariableSymbolLoadOutputData
{
    public bool Result { get; }
    public ISymbolTable<VariableSymbol> Table { get; }
    public Exception? Error { get; }

    public VariableSymbolLoadOutputData( bool result, ISymbolTable<VariableSymbol> table, Exception? error = null )
    {
        Result = result;
        Table  = table;
        Error  = error;
    }
}

public interface IVariableSymbolLoadUseCase
{
    VariableSymbolLoadOutputData Execute();
}
