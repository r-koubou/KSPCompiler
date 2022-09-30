using System;

namespace KSPCompiler.Domain;

public interface ISemanticAnalyser : IDisposable
{
    void Analyse();
    void IDisposable.Dispose() {}
}
