namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record ULongValueObject( uint Value ) : ValueObject<uint>( Value );