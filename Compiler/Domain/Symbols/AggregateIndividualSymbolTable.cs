namespace KSPCompiler.Domain.Symbols;

public abstract class AggregateIndividualSymbolTable<T>( T builtIn, T user )
{
    public T BuiltIn { get; } = builtIn;
    public T User { get; } = user;
}
