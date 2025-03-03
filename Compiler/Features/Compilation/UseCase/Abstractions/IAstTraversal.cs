using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.UseCases.Analysis;

public interface IAstTraversal
{
    void Traverse( AstCompilationUnitNode node );
}
