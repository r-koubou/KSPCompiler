using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

[TestFixture]
public class OutputBinaryOperatorTest
{
    private static void TestBody<TOprNode>( string expected, Action<IAstVisitor, TOprNode> visit ) where TOprNode : AstExpressionNode, new()
    {
        var output = new StringBuilder();
        var evaluator = new NumericBinaryOperatorEvaluator( output );
        var visitor = new MockBinaryOperatorVisitor();
        var expr = new TOprNode();

        visitor.Inject( evaluator );
        visit( visitor, expr );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }

    [Test]
    public void AddTest()
        => TestBody<AstAdditionExpressionNode>( " + ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void SubTest()
        => TestBody<AstSubtractionExpressionNode>( " - ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void MulTest()
        => TestBody<AstMultiplyingExpressionNode>( " * ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void DivTest()
        => TestBody<AstDivisionExpressionNode>( " / ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void ModTest()
        => TestBody<AstModuloExpressionNode>( " mod ", ( visitor, expr ) => visitor.Visit( expr ) );


    [Test]
    public void BitwiseOrTest()
        => TestBody<AstBitwiseOrExpressionNode>( " .or. ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void BitwiseAndTest()
        => TestBody<AstBitwiseAndExpressionNode>( " .and. ", ( visitor, expr ) => visitor.Visit( expr ) );

    [Test]
    public void BitwiseXorTest()
        => TestBody<AstBitwiseXorExpressionNode>( " .xor. ", ( visitor, expr ) => visitor.Visit( expr ) );
}
