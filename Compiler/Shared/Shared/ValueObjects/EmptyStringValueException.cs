// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

using System;

namespace KSPCompiler.Shared.ValueObjects;

public class EmptyStringValueException : Exception
{
    public EmptyStringValueException() {}

    public EmptyStringValueException( string message )
        : base( message ) {}

    public EmptyStringValueException( string name, string message )
        : base( $"{name} - {message}" ) {}

    public static void ThrowIf( StringValueObject value )
    {
        if( !value.AllowEmpty && string.IsNullOrEmpty( value.Value ) )
        {
            throw new EmptyStringValueException();
        }
    }

    public static void ThrowIfWithTrim( StringValueObject value )
    {
        if( !value.AllowEmpty && string.IsNullOrEmpty( value.Value.Trim() ) )
        {
            throw new EmptyStringValueException();
        }
    }

    public static void ThrowIf<TValueObject>( TValueObject value )
        where TValueObject : IValueObject<string>
    {
        if( string.IsNullOrEmpty( value.Value ) )
        {
            throw new EmptyStringValueException();
        }
    }

    public static void ThrowIfWithTrim<TValueObject>( TValueObject value )
        where TValueObject : IValueObject<string>
    {
        if( string.IsNullOrEmpty( value.Value.Trim() ) )
        {
            throw new EmptyStringValueException();
        }
    }
}
