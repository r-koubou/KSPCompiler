using KSPCompiler.Commons.Values;

namespace KSPCompiler.Domain.CompilerMessages;

public sealed record CompilerMessageText( string Value ) : StringValue( Value )
{
    public static implicit operator CompilerMessageText( string value )
        => new( value );
}
