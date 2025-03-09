using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public interface ISymbolFactory<in TAstNode, out TSymbol>
    where TAstNode: AstNode
    where TSymbol : SymbolBase
{
    TSymbol Create( TAstNode node );
}
