using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

public interface IAstTraversal
{
    void Traverse( AstCompilationUnitNode node );
}
