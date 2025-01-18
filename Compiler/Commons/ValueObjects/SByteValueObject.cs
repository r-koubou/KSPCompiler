using System;

namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record SByteValueObject( sbyte Value ) : ValueObject<sbyte>( Value )
{
    public override int GetHashCode()
        => HashCode.Combine( Value );

    public override bool Equals( sbyte other )
        => Value == other;

    public virtual bool Equals( SByteValueObject? other )
        => other is not null && Equals( other.Value );
}
