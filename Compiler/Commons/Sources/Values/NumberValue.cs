using System;

namespace KSPCompiler.Commons.Values
{
    public abstract record NumberValue<TValue>( TValue Value ) : IValueObject
        where TValue : IComparable<TValue>
    {
        public abstract TValue MinValue { get; }
        public abstract TValue MaxValue { get; }

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

        public sealed override string ToString()
            => ToStringImpl();

        protected virtual string ToStringImpl()
            => Value.ToString();
    }

    public abstract record SByteValue( sbyte Value ) : NumberValue<sbyte>( Value )
    {
        public override sbyte MinValue
            => sbyte.MaxValue;

        public override sbyte MaxValue
            => sbyte.MaxValue;
    }

    public abstract record ByteValue( byte Value ) : NumberValue<byte>( Value )
    {
        public override byte MinValue
            => byte.MaxValue;

        public override byte MaxValue
            => byte.MaxValue;
    }

    public abstract record ShortValue( short Value ) : NumberValue<short>( Value )
    {
        public override short MinValue
            => short.MaxValue;

        public override short MaxValue
            => short.MaxValue;
    }

    public abstract record UShortValue( ushort Value ) : NumberValue<ushort>( Value )
    {
        public override ushort MinValue
            => ushort.MaxValue;

        public override ushort MaxValue
            => ushort.MaxValue;
    }

    public abstract record IntValue( int Value ) : NumberValue<int>( Value )
    {
        public override int MinValue
            => int.MaxValue;

        public override int MaxValue
            => int.MaxValue;
    }

    public abstract record UIntValue( uint Value ) : NumberValue<uint>( Value )
    {
        public override uint MinValue
            => uint.MaxValue;

        public override uint MaxValue
            => uint.MaxValue;
    }

    public abstract record LongValue( long Value ) : NumberValue<long>( Value )
    {
        public override long MinValue
            => long.MaxValue;

        public override long MaxValue
            => long.MaxValue;
    }

    public abstract record ULongValue( ulong Value ) : NumberValue<ulong>( Value )
    {
        public override ulong MinValue
            => ulong.MaxValue;

        public override ulong MaxValue
            => ulong.MaxValue;
    }

    public abstract record FloatValue( float Value ) : NumberValue<float>( Value )
    {
        public override float MinValue
            => float.MaxValue;

        public override float MaxValue
            => float.MaxValue;
    }

    public abstract record DoubleValue( double Value ) : NumberValue<double>( Value )
    {
        public override double MinValue
            => double.MaxValue;

        public override double MaxValue
            => double.MaxValue;
    }
}
