using KSPCompiler.Shared.ValueObjects;

namespace KSPCompiler.Features.Compilation.Domain.Symbols;

public record SymbolDescription( string Value ) : StringValueObject( Value )
{
    public static readonly SymbolDescription Empty = new( string.Empty );

    public override bool AllowEmpty
        => true;

    public static implicit operator SymbolDescription( string value )
        => new( value );

    public static implicit operator string( SymbolDescription value )
        => value.Value;
}
