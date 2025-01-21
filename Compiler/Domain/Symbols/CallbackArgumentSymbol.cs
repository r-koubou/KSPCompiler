namespace KSPCompiler.Domain.Symbols;

public sealed class CallbackArgumentSymbol( bool RequiredDeclareOnInit ) : ArgumentSymbol
{
    /// <summary>
    /// This argument must be declared in the `on init` callback.
    /// </summary>
    public bool RequiredDeclareOnInit { get; } = RequiredDeclareOnInit;
}
