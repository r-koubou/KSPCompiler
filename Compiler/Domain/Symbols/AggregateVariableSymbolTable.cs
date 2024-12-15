namespace KSPCompiler.Domain.Symbols;

public sealed class AggregateVariableSymbolTable( IVariableSymbolTable builtIn, IVariableSymbolTable user )
    : AggregateIndividualSymbolTable<IVariableSymbolTable>( builtIn, user );
