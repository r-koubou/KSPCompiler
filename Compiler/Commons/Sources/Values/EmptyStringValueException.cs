using System;

namespace KSPCompiler.Commons.Values
{
    public class EmptyStringValueException : Exception
    {
        public EmptyStringValueException() {}

        public EmptyStringValueException( string message ) : base( message ) {}

        public EmptyStringValueException( string name, string message ) : base( $"{name} - {message}" ) {}

        public static void ThrowIf( StringValue value )
        {
            if( StringValue.IsNullOrEmpty( value ) )
            {
                throw new EmptyStringValueException();
            }
        }

        public static void ThrowIfWithTrim( StringValue value )
        {
            if( StringValue.IsNullOrEmptyWithTrim( value ) )
            {
                throw new EmptyStringValueException();
            }
        }
    }
}
