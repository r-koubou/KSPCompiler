using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Obfuscators;

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

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
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
