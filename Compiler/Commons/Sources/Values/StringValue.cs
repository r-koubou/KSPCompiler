using System;
using System.Collections.Generic;

namespace KSPCompiler.Commons.Values
{
    public abstract record StringValue( string Value ) : IValueObject, IComparable<string>
    {
        public int Length
            => Value.Length;

        public int Count
            => Value.Length;

        private static readonly Dictionary<Type, StringValue> CachedEmpty = new();

        public static bool IsNullOrEmpty( StringValue? value )
            => value is null || string.IsNullOrEmpty( value.Value );

        public static bool IsNullOrEmptyWithTrim( StringValue? value )
        {
            if( value is null || string.IsNullOrEmpty( value.Value ) )
            {
                return true;
            }

            return value.Value.Trim().Length == 0;
        }

        public int CompareTo( string other )
            => string.Compare( Value, other, StringComparison.Ordinal );

        public sealed override string ToString()
            => Value;

        public static implicit operator string( StringValue value )
            => value.Value;
    }
}
