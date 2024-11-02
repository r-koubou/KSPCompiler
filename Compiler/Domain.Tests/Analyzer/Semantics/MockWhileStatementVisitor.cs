using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

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
