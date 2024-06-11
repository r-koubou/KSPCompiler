using System;

using KSPCompiler.Domain.Ast.Node.Blocks;

namespace KSPCompiler.Gateways
{
    /// <summary>
    /// Parses KSP and generates an abstract syntax tree (AST).
    /// </summary>
    /// <remarks>
    /// It is assumed that the actual implementation classes (mainly at the infrastructure layer) will use libraries such as parser generators to convert to AST.
    /// </remarks>
    public interface ISyntaxParser : IDisposable
    {
        public AstCompilationUnit Parse();
        void IDisposable.Dispose() {}
    }
}
