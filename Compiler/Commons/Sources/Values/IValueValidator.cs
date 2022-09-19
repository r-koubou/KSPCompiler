namespace KSPCompiler.Commons.Values
{
    public interface IValueValidator<in T> where T : IValueObject

    {
        void ValidateValue( T value );
    }
}
