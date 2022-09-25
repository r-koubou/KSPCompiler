using System;

namespace KSPCompiler.Commons.Values
{
    public class ValueOutOfRangeException : Exception
    {
        public ValueOutOfRangeException( string message ) : base( message ) {}
        public ValueOutOfRangeException( object actual, string message )
            : base( $"{message} (={actual}" ) {}
    }
}
