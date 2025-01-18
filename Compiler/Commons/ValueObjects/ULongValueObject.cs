using System;

namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record ULongValueObject( ulong Value ) : ValueObject<ulong>( Value )
{
    public override int GetHashCode()
        => HashCode.Combine( Value );

    public override bool Equals( ulong other )
        => Value == other;

    public virtual bool Equals( ULongValueObject? other )
        => other is not null && Equals( other.Value );
}
