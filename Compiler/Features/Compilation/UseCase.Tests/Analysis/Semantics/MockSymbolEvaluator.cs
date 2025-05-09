using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

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
