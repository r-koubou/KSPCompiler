using KSPCompiler.Shared.ValueObjects;

namespace KSPCompiler.Shared.Domain.Symbols;

public record SymbolBuiltIntoVersion( string Value ) : StringValueObject( Value )
{
    public static readonly SymbolBuiltIntoVersion NotAvailable = new( "N/A" );

    public override bool AllowEmpty
        => false;

    public static implicit operator SymbolBuiltIntoVersion( string value )
        => new( value );

    public static implicit operator string( SymbolBuiltIntoVersion value )
        => value.Value;
}
