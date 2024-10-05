using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Symbols;

public interface ISymbolFactory<in TAstNode, out TSymbol>
    where TAstNode: AstNode
    where TSymbol : SymbolBase
{
    TSymbol Create( TAstNode node );
}
