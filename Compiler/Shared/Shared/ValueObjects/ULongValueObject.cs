namespace KSPCompiler.Shared.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record ULongValueObject( ulong Value ) : ValueObject<ulong>( Value );
