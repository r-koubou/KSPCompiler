using System;
using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

[Obsolete]
public sealed class ImportSymbolOutputPortOld<TSymbol> : IOutputPort<IEnumerable<TSymbol>> where TSymbol : SymbolBase
{
    public bool Result { get; }
    public IEnumerable<TSymbol> OutputData { get; }
    public Exception? Error { get; }

    public ImportSymbolOutputPortOld( bool result, IEnumerable<TSymbol> table, Exception? error = null )
    {
        Result     = result;
        OutputData = table;
        Error      = error;
    }
}

[Obsolete]
public interface IImportSymbolUseCaseOld<TSymbol> : IUseCase<UnitInputPort, ImportSymbolOutputPortOld<TSymbol>> where TSymbol : SymbolBase {}
