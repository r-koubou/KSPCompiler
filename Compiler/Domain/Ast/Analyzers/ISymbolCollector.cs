using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers;

public interface ISymbolCollector : IDisposable
{
    public static ISymbolCollector Null { get; } = new NullSymbolCollector();

    ISymbolTable<VariableSymbol> Variables { get; }

    void Analyze( AstCompilationUnit node );
    void IDisposable.Dispose() {}

    private sealed class NullSymbolCollector : ISymbolCollector
    {
        public ISymbolTable<VariableSymbol> Variables => new VariableSymbolTable();

        public void Analyze( AstCompilationUnit node ) {}
    }
}
