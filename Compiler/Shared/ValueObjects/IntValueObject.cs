namespace KSPCompiler.Shared.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record IntValueObject( int Value ) : ValueObject<int>( Value );
