using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

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
