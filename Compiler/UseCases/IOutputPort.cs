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

public sealed class UnitOutputPort( bool result, Exception? error = null ) : IOutputPort<Unit>
{
    public static readonly UnitOutputPort Default = new( true );

    public bool Result { get; } = result;

    public Unit OutputData
        => Unit.Default;

    public Exception? Error { get; } = error;
}
