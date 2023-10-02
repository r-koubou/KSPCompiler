namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record SByteValueObject( sbyte Value ) : ValueObject<sbyte>( Value );