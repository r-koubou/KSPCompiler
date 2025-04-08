namespace KSPCompiler.Shared.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record UIntValueObject( uint Value ) : ValueObject<uint>( Value );
