using KSPCompiler.Commons.ValueObjects;

namespace KSPCompiler.Commons.Text;

public sealed record PlainText( string Value ) : StringValueObject( Value )
{
    public override bool AllowEmpty
        => true;

    public static implicit operator PlainText( string value )
        => new( value );
}
