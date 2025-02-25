using KSPCompiler.Commons;

namespace KSPCompiler.UseCases;

public class InputPort;

public abstract class InputPort<TInput>( TInput handlingInputData ) : InputPort
{
    public TInput HandlingInputData { get; } = handlingInputData;
}

public sealed class UnitInputPort() : InputPort<Unit>( Unit.Default );
