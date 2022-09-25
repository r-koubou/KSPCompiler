using System;

namespace KSPCompiler.Commons.Values
{
    public class EmptyStringException : Exception
    {
        public EmptyStringException() {}

        public EmptyStringException( string message ) : base( message ) {}

        public EmptyStringException( string name, string message ) : base( $"{name} - {message}" ) {}

        public static void ThrowIf( StringValue value )
        {
            if( StringValue.IsNullOrEmpty( value ) )
            {
                throw new EmptyStringException();
            }
        }

        public static void ThrowIfWithTrim( StringValue value )
        {
            if( StringValue.IsNullOrEmptyWithTrim( value ) )
            {
                throw new EmptyStringException();
            }
        }
    }
}
