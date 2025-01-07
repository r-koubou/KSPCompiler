using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.Statements;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockIfStatementVisitor : DefaultAstVisitor
{
    private IIfStatementEvaluator IfStatementEvaluator { get; set; } = new MockIfStatementEvaluator();

    public void Inject( IIfStatementEvaluator evaluator )
        => IfStatementEvaluator = evaluator;

    public override IAstNode Visit( AstIfStatementNode node )
        => IfStatementEvaluator.Evaluate( this, node );
}
