using System;

using KSPCompiler.Domain.Ast.Node.Blocks;

namespace KSPCompiler.Domain.Ast.Analyzers;

public interface ISymbolAnalyzer : IDisposable
{
    void Analyze( AstCompilationUnit node );
    void IDisposable.Dispose() {}
}
