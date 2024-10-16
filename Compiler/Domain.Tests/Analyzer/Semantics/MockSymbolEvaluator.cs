using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

internal class MockSymbolEvaluator : ISymbolEvaluator
{
    private AstExpressionNode EvalResult { get; }

    public MockSymbolEvaluator() : this( NullAstExpressionNode.Instance ) {}

    public MockSymbolEvaluator( AstExpressionNode evalResult )
    {
        EvalResult = evalResult;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstSymbolExpressionNode expr )
        => EvalResult;
}
