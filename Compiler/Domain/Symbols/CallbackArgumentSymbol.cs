namespace KSPCompiler.Domain.Symbols;

public sealed class CallbackArgumentSymbol : VariableSymbol
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
