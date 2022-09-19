using System;

namespace KSPCompiler.Commons.Values
{
    public abstract record StringValue( string Value ) : IValueObject, IComparable<string>
    {
        public virtual bool AllowEmpty
            => true;

        public int Length
            => Value.Length;

        public int Count
            => Value.Length;

        public static bool IsNullOrEmpty( StringValue? value )
            => value is null || string.IsNullOrEmpty( value.Value );

        public int CompareTo( string other )
            => string.Compare( Value, other, StringComparison.Ordinal );

        public sealed override string ToString()
            => Value;
    }
}
