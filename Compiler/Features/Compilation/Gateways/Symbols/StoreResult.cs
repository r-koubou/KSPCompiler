using System;

namespace KSPCompiler.Features.Compilation.Gateways.Symbols;

public sealed class StoreResult
{
    public bool Success { get; }
    public int CreatedCount { get; }
    public int UpdatedCount { get; }
    public int FailedCount { get; }

    public Exception? Exception { get; }

    public StoreResult( bool success = true, int createdCount = 0, int updatedCount = 0, int failedCount = 0, Exception? exception = null )
    {
        Success      = success;
        CreatedCount = createdCount;
        UpdatedCount = updatedCount;
        FailedCount  = failedCount;
        Exception    = exception;
    }

    public override string ToString()
        => $"Success: {Success}, Created: {CreatedCount}, Updated: {UpdatedCount}, Failed: {FailedCount}, Exception: {Exception}";
}
