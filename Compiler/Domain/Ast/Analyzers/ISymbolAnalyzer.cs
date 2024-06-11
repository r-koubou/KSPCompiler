using System;

using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers;

public interface ISymbolAnalyzer : IDisposable
{
    public static ISymbolAnalyzer Null { get; } = new NullSymbolAnalyzer();

    ISymbolTable<VariableSymbol> Variables { get; }

    void Analyze( AstCompilationUnit node );
    void IDisposable.Dispose() {}

    private sealed class NullSymbolAnalyzer : ISymbolAnalyzer
    {
        public ISymbolTable<VariableSymbol> Variables => new VariableSymbolTable();

        public void Analyze( AstCompilationUnit node ) {}
    }
}
