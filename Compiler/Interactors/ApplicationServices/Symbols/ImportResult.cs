using System;

namespace KSPCompiler.Interactors.ApplicationServices.Symbols;

public class ImportResult
{
    public bool Success { get; }
    public int Created { get; }
    public int Updated { get; }
    public int Failed { get; }
    public Exception? Exception { get; }

    public ImportResult( bool success, int created, int updated, int failed, Exception? exception = null )
    {
        Success   = success;
        Created   = created;
        Updated   = updated;
        Failed    = failed;
        Exception = exception;
    }
}
