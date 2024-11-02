using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

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
