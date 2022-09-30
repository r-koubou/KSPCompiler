using System;

namespace KSPCompiler.Domain;

public interface ISymbolAnalyser : IDisposable
{
    void Analyse();
    void IDisposable.Dispose() {}
}
