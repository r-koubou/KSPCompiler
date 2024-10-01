using System;

namespace KSPCompiler.SymbolDatabaseControllers;

public class ExportResult
{
    public bool Success { get; }
    public Exception? Exception { get; }

    public ExportResult( bool success, Exception? exception = null )
    {
        Success   = success;
        Exception = exception;
    }
}
