using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Analyses.Symbol;

public interface ISymbolCollector<TSymbol> where TSymbol : SymbolBase
{
    ISymbolTable<TSymbol> SymbolTable { get; }
    bool HasError { get; }
    ISymbolTable<TSymbol> Collect( AstCompilationUnit root );
}
