using System;

using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Gateways;

public interface ISymbolAnalyzer : IDisposable
{
    public static ISymbolAnalyzer Null { get; } = new NullSymbolAnalyzer();

    ISymbolTable<VariableSymbol> Variables { get; }

    void Analyse( AstCompilationUnit node );
    void IDisposable.Dispose() {}

    private sealed class NullSymbolAnalyzer : ISymbolAnalyzer
    {
        public ISymbolTable<VariableSymbol> Variables => new VariableSymbolTable();

        public void Analyse( AstCompilationUnit node ) {}
    }
}
