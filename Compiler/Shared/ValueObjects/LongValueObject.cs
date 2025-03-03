namespace KSPCompiler.Shared.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record LongValueObject( long Value ) : ValueObject<long>( Value );
