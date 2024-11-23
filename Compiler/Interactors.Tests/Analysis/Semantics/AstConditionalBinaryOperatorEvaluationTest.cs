using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Semantics;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstConditionalBinaryOperatorEvaluationTest
{
    private static void ConditionalBinaryOperatorTestBody<TOperatorNode>( Func<IAstVisitor, TOperatorNode, IAstNode> visit, int expectedErrorCount )
        where TOperatorNode : AstExpressionNode, new()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstConditionalBinaryOperatorVisitor();

        var conditionalBinaryOperatorEvaluator = new ConditionalBinaryOperatorEvaluator(
            compilerMessageManger
        );

        var operatorNode = new TOperatorNode
        {
            Left  = new AstIntLiteralNode( 1 ),
            Right = new AstIntLiteralNode( 1 )
        };

        visitor.Inject( conditionalBinaryOperatorEvaluator );

        var result = visit( visitor, operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        ClassicAssert.AreEqual( expectedErrorCount, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        ClassicAssert.IsNotNull( result );
        ClassicAssert.AreEqual( DataTypeFlag.TypeBool, result?.TypeFlag );
    }

    [Test]
    public void EqualConditionalOperatorTest()
        => ConditionalBinaryOperatorTestBody<AstEqualExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            0
        );


    [Test]
    public void NotEqualConditionalOperatorTest()
        => ConditionalBinaryOperatorTestBody<AstNotEqualExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            0
        );

    [Test]
    public void LessThanConditionalOperatorTest()
        => ConditionalBinaryOperatorTestBody<AstLessThanExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            0
        );

    [Test]
    public void GreaterThanConditionalOperatorTest()
        => ConditionalBinaryOperatorTestBody<AstGreaterThanExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            0
        );

    [Test]
    public void LessEqualConditionalOperatorTest()
        => ConditionalBinaryOperatorTestBody<AstLessEqualExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            0
        );

    [Test]
    public void GreaterEqualConditionalOperatorTest()
        => ConditionalBinaryOperatorTestBody<AstGreaterEqualExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            0
        );

    [Test]
    public void CannotEvaluateIncompatibleTypeTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstConditionalBinaryOperatorVisitor();

        var conditionalBinaryOperatorEvaluator = new ConditionalBinaryOperatorEvaluator(
            compilerMessageManger
        );

        // 1 = 1.0 // error: incompatible types
        var operatorNode = new AstEqualExpressionNode
        {
            Left  = new AstIntLiteralNode( 1 ),
            Right = new AstRealLiteralNode( 1.0 )
        };

        visitor.Inject( conditionalBinaryOperatorEvaluator );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        ClassicAssert.AreEqual( 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        ClassicAssert.IsNotNull( result );
        ClassicAssert.AreEqual( DataTypeFlag.TypeBool, result?.TypeFlag );
    }
}
