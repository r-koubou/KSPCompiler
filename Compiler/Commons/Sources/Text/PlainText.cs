using KSPCompiler.Commons.Values;

namespace KSPCompiler.Commons.Text
{
    public record PlainText( string Value ) : StringValue( Value )
    {
        public static implicit operator PlainText( string value )
            => new( value );

        public static implicit operator string( PlainText value )
            => value.Value;
    }
}
