namespace KSPCompiler.Domain.Symbols;

public sealed class AggregateCallbackSymbolTable( ICallbackSymbolTable builtIn, ICallbackSymbolTable user )
    : AggregateIndividualSymbolTable<ICallbackSymbolTable>( builtIn, user );
