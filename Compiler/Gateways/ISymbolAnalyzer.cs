using System;

namespace KSPCompiler.Gateways;

public interface ISymbolAnalyzer : IDisposable
{
    void Analyse();
    void IDisposable.Dispose() {}
}
