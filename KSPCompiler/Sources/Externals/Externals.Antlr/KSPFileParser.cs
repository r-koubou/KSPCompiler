using System;

using KSPCompiler.Domain.Ast.Blocks;
using KSPCompiler.Domain.Path.Value;
using KSPCompiler.Gateways.Parser;

namespace KSPCompiler.Externals.Antlr
{
    public class KSPFileParser : IKSPParser
    {
        public IPath KspScriptPath { get; }

        public KSPFileParser( IPath scriptPath )
        {
            KspScriptPath = scriptPath;
        }

        public AstCompilationUnit Parse()
        {
            throw new NotImplementedException();
        }
    }
}
