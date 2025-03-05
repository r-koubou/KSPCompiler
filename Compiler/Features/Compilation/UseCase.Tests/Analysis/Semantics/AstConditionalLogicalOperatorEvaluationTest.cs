using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.Compilation.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Symbols.MetaData;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

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
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockAstConditionalLogicalOperatorVisitor();

        var conditionalBinaryOperatorEvaluator = new ConditionalLogicalOperatorEvaluator(
            eventEmitter,
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

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( expectedErrorCount ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( DataTypeFlag.TypeBool ) );
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
                Left     = new AstIntLiteralNode( 1 ),
                Right    = new AstIntLiteralNode( 1 )
            },
            right: new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
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
                Left     = new AstIntLiteralNode( 1 ),
                Right    = new AstIntLiteralNode( 1 )
            },
            right: new AstAdditionExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeInt,
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
