using System;
using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class ImportSymbolOutputPort<TSymbol> : IOutputPort<IEnumerable<TSymbol>> where TSymbol : SymbolBase
{
    public bool Result { get; }
    public IEnumerable<TSymbol> OutputData { get; }
    public Exception? Error { get; }

    public ImportSymbolOutputPort( bool result, IEnumerable<TSymbol> table, Exception? error = null )
    {
        Result     = result;
        OutputData = table;
        Error      = error;
    }
}

public interface IImportSymbolUseCase<TSymbol> : IUseCase<UnitInputPort, ImportSymbolOutputPort<TSymbol>> where TSymbol : SymbolBase {}
