using System;

namespace KSPCompiler.Domain.Ast.Analyzers;

public interface ISymbolAnalyzer : IDisposable
{
    void Analyze();
    void IDisposable.Dispose() {}
}
