using System;

using KSPCompiler.Features.Compilation.Domain;
using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Features.Compilation.Gateways.Parsers
{
    /// <summary>
    /// Parses KSP and generates an abstract syntax tree (AST).
    /// </summary>
    /// <remarks>
    /// It is assumed that the actual implementation classes (mainly at the infrastructure layer) will use libraries such as parser generators to convert to AST.
    /// </remarks>
    public interface ISyntaxParser : IDisposable
    {
        /// <summary>
        /// Parses KSP scripts and generates abstract parse trees.
        /// </summary>
        /// <exception cref="KspCompilerException">If parsing fails</exception>
        public AstCompilationUnitNode Parse();

        /// <inheritdoc cref="IDisposable.Dispose"/>
        void IDisposable.Dispose() {}
    }
}
