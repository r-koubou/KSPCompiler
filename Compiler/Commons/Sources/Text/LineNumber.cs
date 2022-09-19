using System;

using KSPCompiler.Commons.Values;

using RkHelper.Number;

using ValueObjectGenerator;

namespace KSPCompiler.Commons.Text
{
    public record LineNumber( int Value ) : IntValue( Value )
    {
        public static readonly LineNumber Unknown = new( 0 );

        public override int MinValue => 0;
        public override int MaxValue => int.MaxValue;

        protected override string ToStringImpl()
            => ReferenceEquals( Unknown, this ) ? "Unknown" : Value.ToString();

        public static implicit operator LineNumber( int value )
            => new( value );
    }
}
