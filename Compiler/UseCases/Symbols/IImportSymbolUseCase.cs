using System;
using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class SymbolLoadOutputData<TSymbol> : IOutputPort<IEnumerable<TSymbol>> where TSymbol : SymbolBase
{
    public bool Result { get; }
    public IEnumerable<TSymbol> OutputData { get; }
    public Exception? Error { get; }

    public SymbolLoadOutputData( bool result, IEnumerable<TSymbol> table, Exception? error = null )
    {
        Result     = result;
        OutputData = table;
        Error      = error;
    }
}

public interface IImportSymbolUseCase<TSymbol> : IUseCase<Unit, SymbolLoadOutputData<TSymbol>> where TSymbol : SymbolBase {}
