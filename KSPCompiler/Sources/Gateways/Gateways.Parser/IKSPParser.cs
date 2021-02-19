using KSPCompiler.Domain.Ast.Blocks;

namespace KSPCompiler.Gateways.Parser
{
    public interface IKSPParser
    {
        public AstCompilationUnit Parse();
    }
}