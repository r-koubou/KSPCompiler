using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.UserFunctions;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

public class MockCallUserFunctionStatementVisitor : DefaultAstVisitor
{
    private ICallUserFunctionEvaluator Evaluator { get; set; } = new MockCallUserFunctionEvaluator();

    public void Inject( ICallUserFunctionEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstCallUserFunctionStatementNode node )
        => Evaluator.Evaluate( this, node );
}
