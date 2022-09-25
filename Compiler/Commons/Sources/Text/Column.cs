using KSPCompiler.Commons.Values;

namespace KSPCompiler.Commons.Text
{
    public sealed record Column( int Value ) : NumberValue<int>( Value )
    {
        public static readonly Column Unknown = -1;

        protected override string ToStringImpl()
            => ReferenceEquals( Unknown, this ) ? "Unknown" : Value.ToString();

        public static implicit operator Column( int value )
            => new( value );

        public static explicit operator int( Column value )
            => value.Value;
    }
}
