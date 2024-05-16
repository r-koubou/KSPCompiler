using System;

using KSPCompiler.Commons;

namespace KSPCompiler.UseCases;

public interface IOutputPort
{
    public bool Result { get; }
    public Exception? Error { get; }
}

public interface IOutputPort<out TOutputData> : IOutputPort
{
    public TOutputData OutputData { get; }
}

public sealed class UnitOutputPort : IOutputPort<Unit>
{
    public static readonly UnitOutputPort Default = new( true );

    public bool Result { get; }
    public Unit OutputData { get; } = Unit.Default;
    public Exception? Error { get; }

    public UnitOutputPort( bool result, Exception? error = null )
    {
        Result = result;
        Error  = error;
    }
}
