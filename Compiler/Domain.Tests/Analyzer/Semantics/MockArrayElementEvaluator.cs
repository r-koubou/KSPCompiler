using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

internal class MockArrayElementEvaluator : IArrayElementEvaluator
{
    private AstExpressionNode EvalResult { get; }

    public MockArrayElementEvaluator() : this( NullAstExpressionNode.Instance ) {}

    public MockArrayElementEvaluator( AstExpressionNode evalResult )
    {
        EvalResult = evalResult;
    }

    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstArrayElementExpressionNode expr, AbortTraverseToken abortTraverseToken )
        => EvalResult;
}
