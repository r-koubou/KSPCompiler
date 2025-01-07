using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.Statements;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockSelectStatementVisitor : DefaultAstVisitor
{
    private ISelectStatementEvaluator Evaluator { get; set; } = new MockSelectStatementEvaluator();

    public void Inject( ISelectStatementEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstSelectStatementNode node )
        => Evaluator.Evaluate( this, node );
}
