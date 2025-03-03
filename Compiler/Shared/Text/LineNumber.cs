using KSPCompiler.Shared.ValueObjects;

namespace KSPCompiler.Shared.Text;

public sealed record LineNumber( int Value ) : IntValueObject( Value )
{
    public static readonly LineNumber Unknown = -1;

    protected override string ToStringImpl()
        => ReferenceEquals( Unknown, this ) ? "Unknown" : Value.ToString();

    public static implicit operator LineNumber( int value )
        => new( value );
}
