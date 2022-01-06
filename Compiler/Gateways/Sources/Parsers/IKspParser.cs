using KSPCompiler.Domain.Ast.Blocks;

namespace KSPCompiler.Gateways.Parsers
{
    public interface IKspParser
    {
        public AstCompilationUnit Parse();
    }
}