using System;

using KSPCompiler.Domain.Ast.Blocks;

namespace KSPCompiler.Gateways.Parsers
{
    public interface IKspScriptParser : IDisposable
    {
        public AstCompilationUnit Parse();
    }
}