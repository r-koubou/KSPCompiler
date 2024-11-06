using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

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
