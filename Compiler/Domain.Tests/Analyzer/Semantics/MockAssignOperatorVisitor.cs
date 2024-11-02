using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

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
