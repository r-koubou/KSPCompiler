using System;

namespace KSPCompiler.Commons.Values
{
    public abstract record NumberValue<TValue>( TValue Value ) : IValueObject
        where TValue : IComparable<TValue>
    {
        public sealed override string ToString()
            => ToStringImpl();

        protected virtual string ToStringImpl()
            => Value.ToString();

        public static NumberValue<TValue> Min( NumberValue<TValue> a, NumberValue<TValue> b )
            => a < b ? a : b;

        public static NumberValue<TValue> Max( NumberValue<TValue> a, NumberValue<TValue> b )
            => a > b ? a : b;

        public static bool operator <( NumberValue<TValue> left, NumberValue<TValue> right )
            => left.Value.CompareTo( right.Value ) < 0;

        public static bool operator >( NumberValue<TValue> left, NumberValue<TValue> right )
            => left.Value.CompareTo( right.Value ) > 0;

        public static bool operator <=( NumberValue<TValue> left, NumberValue<TValue> right )
            => left.Value.CompareTo( right.Value ) <= 0;

        public static bool operator >=( NumberValue<TValue> left, NumberValue<TValue> right )
            => left.Value.CompareTo( right.Value ) >= 0;
    }
}
