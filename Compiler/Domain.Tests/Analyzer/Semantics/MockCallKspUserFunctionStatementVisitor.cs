using KSPCompiler.Domain.Ast.Analyzers.Evaluators.KspUserFunctions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockCallKspUserFunctionStatementVisitor : DefaultAstVisitor
{
    private ICallKspUserFunctionEvaluator Evaluator { get; set; } = new MockCallKspUserFunctionEvaluator();

    public void Inject( ICallKspUserFunctionEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstCallKspUserFunctionStatementNode node )
        => Evaluator.Evaluate( this, node );
}
