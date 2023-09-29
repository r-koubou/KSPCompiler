namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record DoubleValueObject( double Value ) : ValueObject<double>( Value );