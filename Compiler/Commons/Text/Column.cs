using KSPCompiler.Commons.ValueObjects;

namespace KSPCompiler.Commons.Text;

public record Column( int Value ) : IntValueObject( Value )
{
    public static readonly Column Unknown = -1;

    protected override string ToStringImpl()
        => ReferenceEquals( Unknown, this ) ? "Unknown" : Value.ToString();

    public static implicit operator Column( int value )
        => new( value );
}
