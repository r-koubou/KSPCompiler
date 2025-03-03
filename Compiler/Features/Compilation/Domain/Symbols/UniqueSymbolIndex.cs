using KSPCompiler.Shared.ValueObjects;

namespace KSPCompiler.Features.Compilation.Domain.Symbols;

public record UniqueSymbolIndex( int Value ) : IntValueObject( Value )
{
    public static UniqueSymbolIndex Null => new( -1 );
    public static UniqueSymbolIndex Zero => new( 0 );
}
