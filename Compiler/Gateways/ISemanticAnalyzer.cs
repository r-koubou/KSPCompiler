using System;

namespace KSPCompiler.Gateways;

public interface ISemanticAnalyzer : IDisposable
{
    void Analyze();
    void IDisposable.Dispose() {}
}
