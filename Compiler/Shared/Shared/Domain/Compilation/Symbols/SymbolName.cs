using KSPCompiler.Shared.ValueObjects;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public record SymbolName( string Value ) : StringValueObject( Value )
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly SymbolName Empty = new SymbolName( string.Empty );

    public override bool AllowEmpty
        => ReferenceEquals( Empty, this );

    public static implicit operator SymbolName( string value )
        => new( value );

    public static implicit operator string( SymbolName value )
        => value.Value;
}
