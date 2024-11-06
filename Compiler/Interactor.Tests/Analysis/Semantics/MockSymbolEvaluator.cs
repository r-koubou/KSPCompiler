using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

internal class MockSymbolEvaluator : ISymbolEvaluator
{
    private AstExpressionNode EvalResult { get; }

    public MockSymbolEvaluator() : this( NullAstExpressionNode.Instance ) {}

    public MockSymbolEvaluator( AstExpressionNode evalResult )
    {
        EvalResult = evalResult;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstSymbolExpressionNode expr )
        => EvalResult;
}
