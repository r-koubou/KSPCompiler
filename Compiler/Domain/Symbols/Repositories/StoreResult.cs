using System;

namespace KSPCompiler.Domain.Symbols.Repositories;

public sealed class StoreResult
{
    public bool Success { get; }
    public int CreatedCount { get; }
    public int UpdatedCount { get; }
    public int FailedCount { get; }

    public Exception? Exception { get; }

    public StoreResult( bool success, int createdCount, int updatedCount, int failedCount, Exception? exception = null )
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