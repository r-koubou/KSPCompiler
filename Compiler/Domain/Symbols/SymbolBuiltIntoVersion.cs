using KSPCompiler.Commons.ValueObjects;

namespace KSPCompiler.Domain.Symbols;

public record SymbolBuiltIntoVersion( string Value ) : StringValueObject( Value )
{
    public static readonly SymbolBuiltIntoVersion Empty = new( string.Empty );

    public override bool AllowEmpty
        => true;

    public static implicit operator SymbolBuiltIntoVersion( string value )
        => new( value );

    public static implicit operator string( SymbolBuiltIntoVersion value )
        => value.Value;
}
