using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Analyzers;

public interface IAstTraversal
{
    void Traverse( AstCompilationUnitNode node );
}
