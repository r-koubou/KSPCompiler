using System;

namespace KSPCompiler.Commons.Values
{
    public class ValueOutOfRangeException : Exception
    {
        public ValueOutOfRangeException( string message ) : base( message ) {}

        public ValueOutOfRangeException( object actual, string message )
            : base( $"{message} (={actual}" ) {}

        public static void ThrowIf<T>( NumberValue<T> value, T min, T max ) where T : IComparable<T>
        {
            if( value.Value.CompareTo( min ) < 0 )
            {
                throw new ValueOutOfRangeException( value, $"{nameof( value )}({value}) < {nameof( min )}({min})" );
            }

            if( value.Value.CompareTo( max ) > 0 )
            {
                throw new ValueOutOfRangeException( value, $"{nameof( value )}({value}) > {nameof( max )}({max})" );
            }
        }

        public static void ThrowIf<T>( NumberValue<T> value, IRangeValue<T> range ) where T : IComparable<T>
            => ThrowIf( value, range.MinValue, range.MaxValue );
    }
}
