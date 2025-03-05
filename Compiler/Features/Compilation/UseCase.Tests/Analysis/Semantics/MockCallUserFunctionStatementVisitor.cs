using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.UserFunctions;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

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
