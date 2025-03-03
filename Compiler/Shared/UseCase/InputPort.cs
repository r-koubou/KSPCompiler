using KSPCompiler.Commons;

namespace KSPCompiler.UseCases;

public class InputPort;

public abstract class InputPort<TInput>( TInput input ) : InputPort
{
    public TInput Input { get; } = input;
}

public sealed class UnitInputPort() : InputPort<Unit>( Unit.Default );
