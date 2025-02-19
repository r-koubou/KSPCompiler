using System;

namespace KSPCompiler.Interactors.ApplicationServices.Symbol;

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
