using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

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
