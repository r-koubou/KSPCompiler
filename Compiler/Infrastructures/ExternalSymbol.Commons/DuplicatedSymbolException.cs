using System;

namespace KSPCompiler.ExternalSymbol.Commons;

public sealed class DuplicatedSymbolException : Exception
{
    public DuplicatedSymbolException( string symbolName )
        : base( $"Duplicated symbol : {symbolName}" ) {}
}
