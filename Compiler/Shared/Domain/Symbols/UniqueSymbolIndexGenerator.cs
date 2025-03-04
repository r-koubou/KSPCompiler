using System;

namespace KSPCompiler.Shared.Domain.Symbols;

public class UniqueSymbolIndexGenerator
{
    private int nextIndex;

    public UniqueSymbolIndexGenerator() : this( UniqueSymbolIndex.Zero ) {}

    public UniqueSymbolIndexGenerator(UniqueSymbolIndex startIndex)
    {
        if( startIndex == UniqueSymbolIndex.Null )
        {
            throw new ArgumentException( $"startIndex cannot be {UniqueSymbolIndex.Null}" );
        }

        nextIndex = startIndex.Value;
    }

    public UniqueSymbolIndex Next()
    {
        if( nextIndex == int.MaxValue )
        {
            throw new InvalidOperationException( "SymbolUniqueIndex overflow" );
        }

        return new UniqueSymbolIndex( nextIndex++ );
    }
}
