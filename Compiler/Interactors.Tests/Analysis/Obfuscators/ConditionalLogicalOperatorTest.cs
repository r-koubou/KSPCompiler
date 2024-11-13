using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Interactors.Analysis.Obfuscators;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public class ConditionalLogicalOperatorTest
{
    private static void TestBody<TOprNode>( string expected, Action<IAstVisitor, TOprNode> visit ) where TOprNode : AstExpressionNode, new()
    {
        var output = new StringBuilder();
        var evaluator = new ConditionalLogicalOperatorEvaluator( output );
        var visitor = new MockConditionalLogicalOperatorVisitor();
        var expr = new TOprNode();

        visitor.Inject( evaluator );
        visit( visitor, expr );

        Assert.AreEqual( expected, output.ToString() );
    }

    [Test]
    public void LogicalOrTest()
        => TestBody<AstLogicalOrExpressionNode>( " or ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void LogicalAndTest()
        => TestBody<AstLogicalAndExpressionNode>( " and ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void LogicalXorTest()
        => TestBody<AstLogicalXorExpressionNode>( " xor ", ( visitor, expr ) => visitor.Visit( expr ) );
}
