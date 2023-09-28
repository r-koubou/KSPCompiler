using System;

namespace KSPCompiler.Gateways;

public interface ISymbolAnalyser : IDisposable
{
    void Analyse();
    void IDisposable.Dispose() {}
}
