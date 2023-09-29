using System;

namespace KSPCompiler.Domain.Symbols;

public class UniqueSymbolIndexGenerator
{
    private uint nextIndex;

    public UniqueSymbolIndexGenerator()
    {
        nextIndex = UniqueSymbolIndex.MinValue;
    }

    public UniqueSymbolIndexGenerator(UniqueSymbolIndex startIndex)
    {
        nextIndex = startIndex.Value;
    }

    public UniqueSymbolIndex Next()
    {
        if( nextIndex == UniqueSymbolIndex.Null.Value )
        {
            throw new InvalidOperationException( "SymbolUniqueIndex overflow" );
        }

        return new UniqueSymbolIndex( nextIndex++ );
    }
}
