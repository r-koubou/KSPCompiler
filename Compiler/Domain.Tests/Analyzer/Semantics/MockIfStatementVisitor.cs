using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockIfStatementVisitor : DefaultAstVisitor
{
    private IIfStatementEvaluator IfStatementEvaluator { get; set; } = new MockIfStatementEvaluator();

    public void Inject( IIfStatementEvaluator evaluator )
        => IfStatementEvaluator = evaluator;

    public override IAstNode Visit( AstIfStatementNode node )
        => IfStatementEvaluator.Evaluate( this, node );
}
