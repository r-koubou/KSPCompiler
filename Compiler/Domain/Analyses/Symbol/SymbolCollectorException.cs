using System;

namespace KSPCompiler.Domain.Analyses.Symbol;

public class SymbolCollectorException : Exception
{
    public SymbolCollectorException( string message ) : base( message ) {}
}
