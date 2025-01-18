using System;

namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record ShortValueObject( short Value ) : ValueObject<short>( Value )
{
    public override int GetHashCode()
        => HashCode.Combine( Value );

    public override bool Equals( short other )
        => Value == other;

    public virtual bool Equals( ShortValueObject? other )
        => other is not null && Equals( other.Value );
}
