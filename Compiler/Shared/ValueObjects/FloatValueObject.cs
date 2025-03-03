using System;

namespace KSPCompiler.Shared.ValueObjects;

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

    public virtual bool Equals( FloatValueObject? other )
        => other is not null && Math.Abs( Value - other.Value ) < Epsilon;
}
