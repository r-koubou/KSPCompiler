using KSPCompiler.Commons;

namespace KSPCompiler.UseCases;

public interface IInputPort<out TInput>
{
    public TInput InputData { get; }
}

public sealed class UnitInputPort : IInputPort<Unit>
{
    public Unit InputData => Unit.Default;
}
