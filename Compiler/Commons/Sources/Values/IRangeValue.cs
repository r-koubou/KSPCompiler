namespace KSPCompiler.Commons.Values
{
    public interface IRangeValue<out T>
    {
        T MinValue { get; }
        T MaxValue { get; }
    }
}
