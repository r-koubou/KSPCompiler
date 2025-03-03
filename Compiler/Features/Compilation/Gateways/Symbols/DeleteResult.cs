using System;

namespace KSPCompiler.Features.Compilation.Gateways.Symbols;

public sealed class DeleteResult
{
    public bool Success { get; }
    public int DeletedCount { get; }
    public int FailedCount { get; }

    public Exception? Exception { get; }

    public DeleteResult( bool success = true, int deletedCount = 0, int failedCount = 0, Exception? exception = null )
    {
        Success      = success;
        DeletedCount = deletedCount;
        FailedCount  = failedCount;
        Exception    = exception;
    }

    public override string ToString()
        => $"Success: {Success}, DeletedCount: {DeletedCount}, FailedCount: {FailedCount}, Exception: {Exception}";
}
