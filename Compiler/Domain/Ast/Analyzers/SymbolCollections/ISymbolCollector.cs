namespace KSPCompiler.Domain.Ast.Analyzers.SymbolCollections;

public interface ISymbolCollector<in TNode>
{
    void Collect( TNode node );
}
