using System;
using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class VariableSymbolLoadOutputData : IOutputPort<IEnumerable<VariableSymbol>>
{
    public bool Result { get; }
    public IEnumerable<VariableSymbol> OutputData { get; }
    public Exception? Error { get; }

    public VariableSymbolLoadOutputData( bool result, IEnumerable<VariableSymbol> table, Exception? error = null )
    {
        Result     = result;
        OutputData = table;
        Error      = error;
    }
}

public interface IVariableSymbolLoadUseCase : IUseCase<Unit, VariableSymbolLoadOutputData> {}
