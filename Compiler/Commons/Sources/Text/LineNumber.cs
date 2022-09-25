using KSPCompiler.Commons.Values;

namespace KSPCompiler.Commons.Text
{
    public sealed record LineNumber( int Value ) : NumberValue<int>( Value )
    {
        public static readonly LineNumber Unknown = -1;

        protected override string ToStringImpl()
            => ReferenceEquals( Unknown, this ) ? "Unknown" : Value.ToString();

        public static implicit operator LineNumber( int value )
            => new( value );

        public static explicit operator int( LineNumber value )
            => value.Value;
    }
}
