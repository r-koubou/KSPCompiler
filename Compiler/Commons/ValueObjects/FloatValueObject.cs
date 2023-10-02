namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record FloatValueObject( float Value ) : ValueObject<float>( Value );