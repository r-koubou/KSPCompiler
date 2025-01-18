using System;

namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record UIntValueObject( uint Value ) : ValueObject<uint>( Value )
{
    public override int GetHashCode()
        => HashCode.Combine( Value );

    public override bool Equals( uint other )
        => Value == other;

    public virtual bool Equals( UIntValueObject? other )
        => other is not null && Equals( other.Value );
}
