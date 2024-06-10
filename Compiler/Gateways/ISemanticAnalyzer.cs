using System;

namespace KSPCompiler.Gateways;

public interface ISemanticAnalyzer : IDisposable
{
    void Analyse();
    void IDisposable.Dispose() {}
}
