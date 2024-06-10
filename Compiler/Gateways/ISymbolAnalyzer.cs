using System;

namespace KSPCompiler.Gateways;

public interface ISymbolAnalyzer : IDisposable
{
    void Analyze();
    void IDisposable.Dispose() {}
}
