using KSPCompiler.Commons.Values;

namespace KSPCompiler.Commons.Text
{
    public sealed record PlainText( string Value ) : StringValue( Value )
    {
        public static implicit operator PlainText( string value )
            => new( value );
    }
}
