using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.Conditionals;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

public class MockWhileStatementVisitor : DefaultAstVisitor
{
    private IWhileStatementEvaluator Evaluator { get; set; } = new MockWhileStatementEvaluator();

    public void Inject( IWhileStatementEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstWhileStatementNode node )
        => Evaluator.Evaluate( this, node );
}
