using KSPCompiler.Commons;

namespace KSPCompiler.UseCases;

public interface IInputPort {}

public interface IInputPort<out TInput> : IInputPort
{
    public TInput InputData { get; }
}

public sealed class UnitInputPort : IInputPort<Unit>
{
    public static readonly UnitInputPort Default = new();
    public Unit InputData => Unit.Default;
}
