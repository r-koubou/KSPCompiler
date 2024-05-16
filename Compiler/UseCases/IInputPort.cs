using KSPCompiler.Commons;

namespace KSPCompiler.UseCases;

public interface IInputPort {}

public interface IInputPort<out TInput> : IInputPort
{
    public TInput InputData { get; }
}

public sealed class UnitInputPort : IInputPort<Unit>
{
    public Unit InputData => Unit.Default;
}
