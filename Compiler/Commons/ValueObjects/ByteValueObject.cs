namespace KSPCompiler.Commons.ValueObjects;

// ReSharper disable once UnusedType.Global
public abstract record ByteValueObject( byte Value ) : ValueObject<byte>( Value );
