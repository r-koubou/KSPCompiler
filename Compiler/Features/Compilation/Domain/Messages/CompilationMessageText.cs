using KSPCompiler.Shared.ValueObjects;

namespace KSPCompiler.Features.Compilation.Domain.Messages;

/// <summary>
/// The value object that represents the text of the message.
/// </summary>
public record CompilationMessageText( string Value ) : StringValueObject( Value )
{
    public override bool AllowEmpty
        => false;

    public static implicit operator CompilationMessageText( string text )
        => new( text );
}
