using KSPCompiler.Commons;

namespace KSPCompiler.UseCases;

public class InputPort;

public abstract class InputPort<TInput>( TInput data ) : InputPort
{
    public TInput Data { get; } = data;
}

public sealed class UnitInputPort() : InputPort<Unit>( Unit.Default );
