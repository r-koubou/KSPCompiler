using KSPCompiler.Commons;

namespace KSPCompiler.UseCases;

public class InputPort;

public abstract class InputPort<TInput>( TInput inputData ) : InputPort
{
    public TInput InputData { get; } = inputData;
}

public sealed class UnitInputPort() : InputPort<Unit>( Unit.Default );
