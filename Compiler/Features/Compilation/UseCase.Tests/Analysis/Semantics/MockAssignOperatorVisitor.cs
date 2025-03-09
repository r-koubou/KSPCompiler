using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

public class MockAssignOperatorVisitor : DefaultAstVisitor
{
    private IAssignOperatorEvaluator AssignOperatorEvaluator { get; set; } = new MockAssignOperatorEvaluator();

    public void Inject( IAssignOperatorEvaluator evaluator )
    {
        AssignOperatorEvaluator = evaluator;
    }

    public override IAstNode Visit( AstAssignmentExpressionNode node )
        => AssignOperatorEvaluator.Evaluate( this, node );
}
