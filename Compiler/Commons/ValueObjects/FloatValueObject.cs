using System;

namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record FloatValueObject( float Value ) : ValueObject<float>( Value )
{
    private const float DefaultEpsilon = 1e-10f;

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public static float Epsilon { get; set; }
        = DefaultEpsilon;

    public override int GetHashCode()
        => HashCode.Combine( Value );

    public override bool Equals( float other )
        => Math.Abs( Value - other ) < Epsilon;

    public virtual bool Equals( FloatValueObject? other )
        => other is not null && Equals( other.Value );
}
