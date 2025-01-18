using System;

namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record UShortValueObject( ushort Value ) : ValueObject<ushort>( Value )
{
    public override int GetHashCode()
        => HashCode.Combine( Value );

    public override bool Equals( ushort other )
        => Value == other;

    public virtual bool Equals( UShortValueObject? other )
        => other is not null && Equals( other.Value );
}
