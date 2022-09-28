using KSPCompiler.Commons.ValueObjects;

namespace KSPCompiler.Domain.CompilerMessages;

public record CompilerMessageText( string Value ) : StringValueObject( Value )
{
    public override bool AllowEmpty
        => false;

    public static implicit operator CompilerMessageText( string text )
        => new( text );
}
