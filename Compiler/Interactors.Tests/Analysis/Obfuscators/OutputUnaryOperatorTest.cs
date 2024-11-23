using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Interactors.Analysis.Obfuscators;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class OutputUnaryOperatorTest
{
    private static void TestBody<TOprNode>( string expected, Action<IAstVisitor, TOprNode> visit ) where TOprNode : AstExpressionNode, new()
    {
        var output = new StringBuilder();
        var evaluator = new NumericUnaryOperatorEvaluator( output );
        var visitor = new MockUnaryOperatorVisitor();
        var expr = new TOprNode();

        visitor.Inject( evaluator );
        visit( visitor, expr );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }

    private static (IOperatorEvaluator, StringBuilder) CreateEvaluator()
    {
        var output = new StringBuilder();
        var evaluator = new NumericUnaryOperatorEvaluator( output );
        return ( evaluator, output );
    }

    [Test]
    public void UnaryMinusTest()
        => TestBody<AstUnaryMinusExpressionNode>( " - ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void NotTest()
        => TestBody<AstUnaryNotExpressionNode>( " .not. ", ( visitor, expr ) => visitor.Visit( expr ) );
}
