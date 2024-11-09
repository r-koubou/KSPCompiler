namespace KSPCompiler.Domain.Symbols;

public sealed class CallbackArgumentSymbol : ArgumentSymbol
{
    /// <summary>
    /// This argument must be declared in the `on init` callback.
    /// </summary>
    public bool RequiredDeclareOnInit { get; }

    public CallbackArgumentSymbol( bool requiredDeclareOnInit )
    {
        RequiredDeclareOnInit = requiredDeclareOnInit;
    }
}
