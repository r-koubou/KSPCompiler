using System;

namespace KSPCompiler.Commons.ValueObjects;

public abstract record StringValueObject( string Value ) : ValueObject<string>( Value )
{
    public abstract bool AllowEmpty { get; }

    public override int GetHashCode()
        => HashCode.Combine( Value );

    public override bool Equals( string other )
        => Value == other;

    public virtual bool Equals( StringValueObject? other )
        => other is not null && Equals( other.Value );
}
