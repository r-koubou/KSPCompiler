using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

public class MockSymbolEvaluatorVisitor : DefaultAstVisitor
{
    private ISymbolEvaluator Evaluator { get; set; } = new MockSymbolEvaluator();

    public void Inject( ISymbolEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstSymbolExpressionNode node )
        => Evaluator.Evaluate( this, node );

    private class MockSymbolEvaluator : ISymbolEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstSymbolExpressionNode expr )
            => throw new NotImplementedException();
    }
}
