using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

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
            => throw new System.NotImplementedException();
    }
}
