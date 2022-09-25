using System;

namespace KSPCompiler.Commons.Values
{
    public static class NumberHelper
    {
        #region Validation
        public static void ValidateRange<T>( NumberValue<T> value, T min, T max ) where T : IComparable<T>
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

        public static void ValidateRange<T>( NumberValue<T> value, IRangeValue<T> range ) where T : IComparable<T>
            => ValidateRange( value, range.MinValue, range.MaxValue );

        public static bool InRange<T>( NumberValue<T> value, T min, T max ) where T : IComparable<T>
        {
            if( min.CompareTo( max ) > 0 ||
                value.Value.CompareTo( min ) < 0 ||
                value.Value.CompareTo( max ) > 0 )
            {
                return false;
            }

            return true;
        }

        public static void InRange<T>( NumberValue<T> value, IRangeValue<T> range ) where T : IComparable<T>
            => InRange( value, range.MinValue, range.MaxValue );
        #endregion
    }
}
