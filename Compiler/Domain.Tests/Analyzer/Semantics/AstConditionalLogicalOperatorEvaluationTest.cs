using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;
using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstConditionalLogicalOperatorEvaluationTest
{
    #region Common code implementation of each conditional logical operator

    private static void ConditionalLogicalOperatorTestBody<TOperatorNode>(
        Func<IAstVisitor, TOperatorNode, IAstNode> visit,
        int expectedErrorCount,
        AstExpressionNode left,
        AstExpressionNode right )
        where TOperatorNode : AstExpressionNode, new()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstConditionalLogicalOperatorVisitor();

        var conditionalBinaryOperatorEvaluator = new ConditionalLogicalOperatorEvaluator(
            compilerMessageManger,
            MockUtility.CreateBooleanConvolutionEvaluator( visitor )
        );

        // left <opr> right
        var operatorNode = new TOperatorNode
        {
            Left  = left,
            Right = right
        };

        operatorNode.Left.Parent = operatorNode;

        visitor.Inject( conditionalBinaryOperatorEvaluator );

        var result = visit( visitor, operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( expectedErrorCount, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.IsNotNull( result );
        Assert.AreEqual( DataTypeFlag.TypeBool, result?.TypeFlag );
    }

    private static void ConditionalLogicalOperatorTestBody<TOperatorNode>( Func<IAstVisitor, TOperatorNode, IAstNode> visit, int expectedErrorCount )
        where TOperatorNode : AstExpressionNode, new()
    {
        ConditionalLogicalOperatorTestBody(
            visit: visit,
            expectedErrorCount: expectedErrorCount,
            left: new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Constant = true,
                Left     = new AstIntLiteralNode( 1 ),
                Right    = new AstIntLiteralNode( 1 )
            },
            right: new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Constant = true,
                Left     = new AstIntLiteralNode( 2 ),
                Right    = new AstIntLiteralNode( 2 )
            }
        );
    }

    private static void CannotIncompatibleTypeConditionalLogicalOperatorTestBody<TOperatorNode>( Func<IAstVisitor, TOperatorNode, IAstNode> visit, int expectedErrorCount )
        where TOperatorNode : AstExpressionNode, new()
    {
        ConditionalLogicalOperatorTestBody(
            visit: visit,
            expectedErrorCount: expectedErrorCount,
            // 1 = 1 <opr> 2 + 2 <-- `2 + 2` is not boolean
            left: new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Constant = true,
                Left     = new AstIntLiteralNode( 1 ),
                Right    = new AstIntLiteralNode( 1 )
            },
            right: new AstAdditionExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeInt,
                Constant = true,
                Left     = new AstIntLiteralNode( 2 ),
                Right    = new AstIntLiteralNode( 2 )
            }
        );
    }

    #endregion ~Common code implementation of each conditional logical operator

    [Test]
    public void LogicalAndConditionalOperatorTest()
        => ConditionalLogicalOperatorTestBody<AstLogicalAndExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            0
        );

    [Test]
    public void LogicalOrConditionalOperatorTest()
        => ConditionalLogicalOperatorTestBody<AstLogicalOrExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            0
        );

    [Test]
    public void LogicalXorConditionalOperatorTest()
        => ConditionalLogicalOperatorTestBody<AstLogicalXorExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            0
        );

    [Test]
    public void CannotLogicalAndConditionalOperatorWithIncompatibleTest()
        => CannotIncompatibleTypeConditionalLogicalOperatorTestBody<AstLogicalAndExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            1
        );

    [Test]
    public void CannotLogicalOrConditionalOperatorWithIncompatibleTest()
        => CannotIncompatibleTypeConditionalLogicalOperatorTestBody<AstLogicalOrExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            1
        );

    [Test]
    public void CannotLogicalXorConditionalOperatorWithIncompatibleTest()
        => CannotIncompatibleTypeConditionalLogicalOperatorTestBody<AstLogicalXorExpressionNode>(
            ( visitor, node ) => visitor.Visit( node ),
            1
        );
}
