using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public class MockArrayElementEvaluatorVisitor : DefaultAstVisitor
{
    private StringBuilder Output { get;}
    private IArrayElementEvaluator Evaluator { get; set; } = new MockArrayElementEvaluator();

    public MockArrayElementEvaluatorVisitor( StringBuilder output )
    {
        Output = output;
    }

    public void Inject( IArrayElementEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstArrayElementExpressionNode expr )
        => Evaluator.Evaluate( this, expr );

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        Output.Append( node.Name );
        return node;
    }

    public override IAstNode Visit( AstIntLiteralNode node )
    {
        Output.Append( $"{node.Value}" );
        return node;
    }

    private class MockArrayElementEvaluator : IArrayElementEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstArrayElementExpressionNode expr )
            => throw new NotImplementedException();
    }
}
