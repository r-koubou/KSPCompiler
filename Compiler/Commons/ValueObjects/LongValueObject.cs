namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record LongValueObject( int Value ) : ValueObject<int>( Value );