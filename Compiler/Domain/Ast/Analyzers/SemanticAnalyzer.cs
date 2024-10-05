using KSPCompiler.Domain.Ast.Node;
using KSPCompiler.Domain.Ast.Node.Blocks;

namespace KSPCompiler.Domain.Ast.Analyzers;

public partial class SemanticAnalyzer : DefaultAstVisitor, ISemanticAnalyzer
{
    public void Analyze( AstCompilationUnit node )
    {
        node.AcceptChildren( this );
    }
}
