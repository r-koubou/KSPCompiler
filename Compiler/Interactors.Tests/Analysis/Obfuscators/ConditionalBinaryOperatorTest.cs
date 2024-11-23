using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Interactors.Analysis.Obfuscators;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class ConditionalBinaryOperatorTest
{
    private static void TestBody<TOprNode>( string expected, Action<IAstVisitor, TOprNode> visit ) where TOprNode : AstExpressionNode, new()
    {
        var output = new StringBuilder();
        var evaluator = new ConditionalBinaryOperatorEvaluator( output );
        var visitor = new MockConditionalBinaryOperatorVisitor();
        var expr = new TOprNode();

        visitor.Inject( evaluator );
        visit( visitor, expr );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }

    [Test]
    public void EqualTest()
        => TestBody<AstEqualExpressionNode>( " = ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void NotEqualTest()
        => TestBody<AstNotEqualExpressionNode>( " # ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void LessThanTest()
        => TestBody<AstLessThanExpressionNode>( " < ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void GreaterThanTest()
        => TestBody<AstGreaterThanExpressionNode>( " > ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void LessThanOrEqualTest()
        => TestBody<AstLessEqualExpressionNode>( " <= ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void GreaterThanOrEqualTest()
        => TestBody<AstGreaterEqualExpressionNode>( " >= ", ( visitor, expr ) => visitor.Visit( expr ) );

}
