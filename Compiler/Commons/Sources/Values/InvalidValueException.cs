using System;

namespace KSPCompiler.Commons.Values
{
    public sealed class InvalidValueException : Exception
    {
        public InvalidValueException( string name, IValueObject actual, string message ) : base( $"{name} : {message} (actual = {actual}" ) {}
        public InvalidValueException( IValueObject actual, string message ) : base( $"{message} (actual = {actual}" ) {}
        public InvalidValueException( string message ) : base( message ) {}
        public InvalidValueException() {}
    }
}
