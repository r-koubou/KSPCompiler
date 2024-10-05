using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Analyzers;

public interface ISemanticAnalyzer : IDisposable
{
    void Analyze( AstCompilationUnit node );
    void IDisposable.Dispose() {}
}
