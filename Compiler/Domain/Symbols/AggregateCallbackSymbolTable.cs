namespace KSPCompiler.Domain.Symbols;

public sealed class AggregateCallbackSymbolTable( ICallbackSymbolTable builtIn, ICallbackSymbolTable user )
{
    public ICallbackSymbolTable BuiltIn { get; } = builtIn;
    public ICallbackSymbolTable User { get; } = user;
}
