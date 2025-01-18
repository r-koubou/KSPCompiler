using System;

namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record LongValueObject( long Value ) : ValueObject<long>( Value )
{
    public override int GetHashCode()
        => HashCode.Combine( Value );

    public override bool Equals( long other )
        => Value == other;

    public virtual bool Equals( LongValueObject? other )
        => other is not null && Equals( other.Value );
}
