using System;

namespace KSPCompiler.Gateways;

public interface ISemanticAnalyser : IDisposable
{
    void Analyse();
    void IDisposable.Dispose() {}
}
