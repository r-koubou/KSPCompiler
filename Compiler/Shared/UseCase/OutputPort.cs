using System;

using KSPCompiler.Commons;

namespace KSPCompiler.UseCases;

public abstract class OutputPort( bool result, Exception? error = null )
{
    public bool Result { get; } = result;
    public Exception? Error { get; } = error;
}

public abstract class OutputPort<TOutputData>(
    TOutputData outputData,
    bool result,
    Exception? error = null
) : OutputPort( result, error )
{
    public TOutputData OutputData { get; } = outputData;
}

public sealed class UnitOutputPort(
    bool result,
    Exception? error = null
) : OutputPort<Unit>( Unit.Default, result, error );
