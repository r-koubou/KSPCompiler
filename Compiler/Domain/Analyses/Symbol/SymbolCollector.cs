using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Analyses.Symbol;

public class SymbolCollector : ISymbolCollector
{
    public SymbolTable<VariableSymbol> VariableTable { get; } = new VariableSymbolTable();
    public SymbolTable<FunctionSymbol> FunctionTable { get; } = new FunctionSymbolTable();

    public void Collect( AstCompilationUnit root )
    {
        throw new System.NotImplementedException();
    }
}
