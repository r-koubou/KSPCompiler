using System;

namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record ByteValueObject( byte Value ) : ValueObject<byte>( Value )
{
    public override int GetHashCode()
        => HashCode.Combine( Value );

    public override bool Equals( byte other )
        => Value == other;

    public virtual bool Equals( ByteValueObject? other )
        => other is not null && Equals( other.Value );
}
