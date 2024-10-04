using System;
using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.SymbolDatabaseControllers;

public sealed class FindResult<TSymbol> where TSymbol : SymbolBase
{
    public bool Success { get; }
    public IReadOnlyCollection<TSymbol> FoundSymbols { get; }
    public Exception? Exception { get; }

    public FindResult( bool success, IReadOnlyCollection<TSymbol> foundSymbols, Exception? exception = null )
    {
        Success      = success;
        FoundSymbols = foundSymbols;
        Exception    = exception;
    }
}
