using System;

using KSPCompiler.Commons;

namespace KSPCompiler.UseCases;

public interface IOutputPort
{
    public bool Result { get; }
    public Exception? Error { get; }
}

public interface IOutputPort<out TOutputData>
{
    public bool Result { get; }
    public TOutputData OutputData { get; }
    public Exception? Error { get; }


}

public sealed class UnitOutputPort : IOutputPort<Unit>
{
    public bool Result { get; }
    public Unit OutputData { get; } = Unit.Default;
    public Exception? Error { get; }

    public UnitOutputPort( bool result, Exception? error )
    {
        Result     = result;
        Error      = error;
    }
}
