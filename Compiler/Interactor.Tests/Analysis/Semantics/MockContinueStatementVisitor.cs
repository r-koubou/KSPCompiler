using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.Conditionals;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

public class MockContinueStatementVisitor : DefaultAstVisitor
{
    private IContinueStatementEvaluator Evaluator { get; set; } = new MockContinueStatementEvaluator();

    public void Inject( IContinueStatementEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstContinueStatementNode node )
        => Evaluator.Evaluate( this, node );
}
