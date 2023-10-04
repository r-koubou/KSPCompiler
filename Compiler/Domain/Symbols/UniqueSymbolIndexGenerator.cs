using System;

namespace KSPCompiler.Domain.Symbols;

public class UniqueSymbolIndexGenerator
{
    private uint nextIndex;

    public UniqueSymbolIndexGenerator() : this( UniqueSymbolIndex.Zero ) {}

    public UniqueSymbolIndexGenerator(UniqueSymbolIndex startIndex)
    {
        nextIndex = startIndex.Value;
    }

    public UniqueSymbolIndex Next()
    {
        if( nextIndex == uint.MaxValue )
        {
            throw new InvalidOperationException( "SymbolUniqueIndex overflow" );
        }

        return new UniqueSymbolIndex( nextIndex++ );
    }
}
