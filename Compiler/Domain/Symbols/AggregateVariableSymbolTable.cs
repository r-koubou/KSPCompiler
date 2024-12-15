namespace KSPCompiler.Domain.Symbols;

public sealed class AggregateVariableSymbolTable( IVariableSymbolTable builtIn, IVariableSymbolTable user )
{
    public IVariableSymbolTable BuiltIn { get; } = builtIn;
    public IVariableSymbolTable User { get; } = user;
}
