using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Commands;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

public class MockCallCommandExpressionVisitor : DefaultAstVisitor
{
    private ICallCommandEvaluator CallCommandEvaluator { get; set; } = new MockICallCommandEvaluator();

    public void Inject( ICallCommandEvaluator iCallCommandEvaluator )
    {
        CallCommandEvaluator = iCallCommandEvaluator;
    }

    public override IAstNode Visit( AstCallCommandExpressionNode node )
        => CallCommandEvaluator.Evaluate( this, node );
}
