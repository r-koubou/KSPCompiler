using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

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
