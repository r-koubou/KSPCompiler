using KSPCompiler.Commons.Values;

namespace KSPCompiler.Domain.Ast.Symbols;

public sealed record SymbolName( string Value ) : StringValue( Value )
{
    public static readonly SymbolName Empty = new( string.Empty );

    public static implicit operator SymbolName( string name )
        => new( name );
}
