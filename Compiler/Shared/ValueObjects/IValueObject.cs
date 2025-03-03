namespace KSPCompiler.Commons.ValueObjects;

public interface IValueObject<TValue> where TValue : notnull
{
    TValue Value { get; init; }
}
