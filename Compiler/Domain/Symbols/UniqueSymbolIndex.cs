using KSPCompiler.Commons.ValueObjects;

namespace KSPCompiler.Domain.Symbols;

public record UniqueSymbolIndex( uint Value ) : UIntValueObject( Value )
{
    public static UniqueSymbolIndex Zero => new( 0 );
}
