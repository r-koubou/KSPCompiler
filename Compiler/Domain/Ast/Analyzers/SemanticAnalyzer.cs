using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Analyzers;

public partial class SemanticAnalyzer : DefaultAstVisitor, ISemanticAnalyzer
{
    public void Analyze( AstCompilationUnit node )
    {
        node.AcceptChildren( this );
    }
}
