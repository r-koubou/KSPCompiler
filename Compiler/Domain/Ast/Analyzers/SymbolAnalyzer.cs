using System;

using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers;

public class SymbolAnalyzer : ISymbolAnalyzer
{
    #region Symbol Tables
    public ISymbolTable<VariableSymbol> Variables { get; } = new VariableSymbolTable();
    #endregion

    public void Analyze( AstCompilationUnit node )
    {
        throw new NotImplementedException();
    }
}
