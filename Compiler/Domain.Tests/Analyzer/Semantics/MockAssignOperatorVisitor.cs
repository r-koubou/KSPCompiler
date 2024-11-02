using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Assigns;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockAssignOperatorVisitor : DefaultAstVisitor
{
    private IAssignStatementEvaluator AssignStatementEvaluator { get; set; } = new MockAssignStatementEvaluator();

    public void Inject( IAssignStatementEvaluator evaluator )
    {
        AssignStatementEvaluator = evaluator;
    }

    public override IAstNode Visit( AstAssignStatementNode node )
        => AssignStatementEvaluator.Evaluate( this, node );
}
