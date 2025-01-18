using System;

namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record IntValueObject( int Value ) : ValueObject<int>( Value )
{
    public override int GetHashCode()
        => HashCode.Combine( Value );

    public override bool Equals( int other )
        => Value == other;

    public virtual bool Equals( IntValueObject? other )
        => other is not null && Equals( other.Value );
}
