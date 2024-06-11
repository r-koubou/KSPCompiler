using System;

using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Parser.Symbols;

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
