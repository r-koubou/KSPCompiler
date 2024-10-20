using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers;

public interface ISymbolCollector : IDisposable
{
    public static ISymbolCollector Null { get; } = new NullSymbolCollector();

    void Analyze( AstCompilationUnitNode node );
    void IDisposable.Dispose() {}

    private sealed class NullSymbolCollector : ISymbolCollector
    {
        public IVariableSymbolTable Variables => new VariableSymbolTable();

        public void Analyze( AstCompilationUnitNode node ) {}
    }
}
