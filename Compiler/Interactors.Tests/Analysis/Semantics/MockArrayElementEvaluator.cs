using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

internal class MockArrayElementEvaluator : IArrayElementEvaluator
{
    private AstExpressionNode EvalResult { get; }

    public MockArrayElementEvaluator() : this( NullAstExpressionNode.Instance ) {}

    public MockArrayElementEvaluator( AstExpressionNode evalResult )
    {
        EvalResult = evalResult;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstArrayElementExpressionNode expr )
        => EvalResult;
}
