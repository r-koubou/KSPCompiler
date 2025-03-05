using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

public class MockIfStatementVisitor : DefaultAstVisitor
{
    private IIfStatementEvaluator IfStatementEvaluator { get; set; } = new MockIfStatementEvaluator();

    public void Inject( IIfStatementEvaluator evaluator )
        => IfStatementEvaluator = evaluator;

    public override IAstNode Visit( AstIfStatementNode node )
        => IfStatementEvaluator.Evaluate( this, node );
}
