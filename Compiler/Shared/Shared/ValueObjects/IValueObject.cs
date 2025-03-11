namespace KSPCompiler.Shared.ValueObjects;

public interface IValueObject<TValue> where TValue : notnull
{
    TValue Value { get; init; }
}
