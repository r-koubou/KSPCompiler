using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Analyses.Symbol;

public interface ISymbolCollector
{
    SymbolTable Collect( AstCompilationUnit root );
}
