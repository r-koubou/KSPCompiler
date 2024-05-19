using System;

using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways;

namespace KSPCompiler.Parser.Symbols;

public class SymbolAnalyzer : ISymbolAnalyzer
{
    #region Symbol Tables
    private ISymbolTable<VariableSymbol>? variables;

    public ISymbolTable<VariableSymbol> Variables
    {
        get
        {
            var result = new VariableSymbolTable();

            if( variables != null )
            {
                result.AddRange( variables );
            }

            return result;
        }
    }

    #endregion

    public void Analyse( AstCompilationUnit node )
    {
        throw new NotImplementedException();
    }
}
