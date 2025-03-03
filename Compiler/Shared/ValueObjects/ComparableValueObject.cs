using System;

namespace KSPCompiler.Shared.ValueObjects;

public abstract record ComparableValueObject<TValue>( TValue Value )
    : ValueObject<TValue>( Value ), IComparable<ComparableValueObject<TValue>>
    where TValue : IComparable<TValue>
{
    public int CompareTo( ComparableValueObject<TValue> other )
        => Value.CompareTo( other.Value );

    public static bool operator <( ComparableValueObject<TValue> left, ComparableValueObject<TValue> right )
        => left.CompareTo( right ) < 0;

    public static bool operator >( ComparableValueObject<TValue> left, ComparableValueObject<TValue> right )
        => left.CompareTo( right ) > 0;

    public static bool operator <=( ComparableValueObject<TValue> left, ComparableValueObject<TValue> right )
        => left.CompareTo( right ) <= 0;

    public static bool operator >=( ComparableValueObject<TValue> left, ComparableValueObject<TValue> right )
        => left.CompareTo( right ) >= 0;
}
