using System;

using KSPCompiler.Domain.Ast.Node.Blocks;

namespace KSPCompiler.Gateways
{
    public interface ISyntaxAnalyser : IDisposable
    {
        public AstCompilationUnit Analyse();
        void IDisposable.Dispose() {}
    }
}
