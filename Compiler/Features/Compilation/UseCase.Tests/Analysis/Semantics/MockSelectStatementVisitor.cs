using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

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
