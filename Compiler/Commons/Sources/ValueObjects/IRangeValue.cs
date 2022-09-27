namespace KSPCompiler.Commons.ValueObjects;

public interface IRangeValue<out TValue>
{
    TValue MinValue { get; }
    TValue MaxValue { get; }
}
