namespace KSPCompiler.Domain.Symbols;

public sealed record CallbackArgumentSymbol( bool RequiredDeclareOnInit ) : ArgumentSymbol
{
    /// <summary>
    /// This argument must be declared in the `on init` callback.
    /// </summary>
    public bool RequiredDeclareOnInit { get; } = RequiredDeclareOnInit;
}
