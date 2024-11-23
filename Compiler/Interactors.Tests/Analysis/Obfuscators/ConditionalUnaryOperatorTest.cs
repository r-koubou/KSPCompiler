using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Interactors.Analysis.Obfuscators;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class ConditionalUnaryOperatorTest
{
    private static void TestBody<TOprNode>( string expected, Action<IAstVisitor, TOprNode> visit ) where TOprNode : AstExpressionNode, new()
    {
        var output = new StringBuilder();
        var evaluator = new ConditionalUnaryOperatorEvaluator( output );
        var visitor = new MockConditionalUnaryOperatorVisitor();
        var expr = new TOprNode();

        visitor.Inject( evaluator );
        visit( visitor, expr );

        ClassicAssert.AreEqual( expected, output.ToString() );
    }

    [Test]
    public void LogicalNotTest()
        => TestBody<AstUnaryLogicalNotExpressionNode>( " not ", ( visitor, expr ) => visitor.Visit( expr ) );

}
