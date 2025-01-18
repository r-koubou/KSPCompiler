using System;

namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record DoubleValueObject( double Value ) : ValueObject<double>( Value )
{
    private const double DefaultEpsilon = 1e-10;

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public static double Epsilon { get; set; }
        = DefaultEpsilon;

    public override int GetHashCode()
        => HashCode.Combine( Value );

    public override bool Equals( double other )
        => Math.Abs( Value - other ) < Epsilon;

    public virtual bool Equals( DoubleValueObject? other )
        => other is not null && Equals( other.Value );
}
