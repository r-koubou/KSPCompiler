using System;

namespace KSPCompiler.Domain.Ast.Analyzers;

public interface ISemanticAnalyzer : IDisposable
{
    void Analyze();
    void IDisposable.Dispose() {}
}
