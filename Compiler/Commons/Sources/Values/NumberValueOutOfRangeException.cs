using System;

namespace KSPCompiler.Commons.Values
{
    public class NumberValueOutOfRangeException : Exception
    {
        public NumberValueOutOfRangeException( string message ) : base( message ) {}

        public NumberValueOutOfRangeException( object actual, string message )
            : base( $"{message} (={actual}" ) {}

        public static void ThrowIf<T>( NumberValue<T> value, T min, T max ) where T : IComparable<T>
        {
            if( value.Value.CompareTo( min ) < 0 )
            {
                throw new NumberValueOutOfRangeException( value, $"{nameof( value )}({value}) < {nameof( min )}({min})" );
            }

            if( value.Value.CompareTo( max ) > 0 )
            {
                throw new NumberValueOutOfRangeException( value, $"{nameof( value )}({value}) > {nameof( max )}({max})" );
            }
        }

        public static void ThrowIf<T>( NumberValue<T> value, IRangeValue<T> range ) where T : IComparable<T>
            => ThrowIf( value, range.MinValue, range.MaxValue );
    }
}
