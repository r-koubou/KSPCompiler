using KSPCompiler.Commons.ValueObjects;

namespace KSPCompiler.Domain.CompilerMessages;

/// <summary>
/// The value object that represents the text of the message.
/// </summary>
public record CompilerMessageText( string Value ) : StringValueObject( Value )
{
    public override bool AllowEmpty
        => false;

    public static implicit operator CompilerMessageText( string text )
        => new( text );
}
