using System;

namespace KSPCompiler.SymbolDatabaseControllers;

public sealed class DeleteResult
{
    public bool Success { get; }
    public int DeletedCount { get; }
    public Exception? Exception { get; }

    public DeleteResult( bool success, int deletedCount, Exception? exception )
    {
        Success      = success;
        DeletedCount = deletedCount;
        Exception    = exception;
    }
}
