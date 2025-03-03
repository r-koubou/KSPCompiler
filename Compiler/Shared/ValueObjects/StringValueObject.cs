namespace KSPCompiler.Shared.ValueObjects;

public abstract record StringValueObject( string Value ) : ValueObject<string>( Value )
{
    public abstract bool AllowEmpty { get; }
}
