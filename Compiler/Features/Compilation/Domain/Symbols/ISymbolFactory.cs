using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.Domain.Symbols;

public interface ISymbolFactory<in TAstNode, out TSymbol>
    where TAstNode: AstNode
    where TSymbol : SymbolBase
{
    TSymbol Create( TAstNode node );
}
