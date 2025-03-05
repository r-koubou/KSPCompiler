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
public class AstConditionalBinaryOperatorEvaluationTest
{
    private static void ConditionalBinaryOperatorTestBody<TOperatorNode>( Func<IAstVisitor, TOperatorNode, IAstNode> visit, int expectedErrorCount )
        where TOperatorNode : AstExpressionNode, new()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockAstConditionalBinaryOperatorVisitor();

        var conditionalBinaryOperatorEvaluator = new ConditionalBinaryOperatorEvaluator(
            eventEmitter
        );

        var operatorNode = new TOperatorNode
        {
            Left  = new AstIntLiteralNode( 1 ),
            Right = new AstIntLiteralNode( 1 )
        };

        visitor.Inject( conditionalBinaryOperatorEvaluator );

        var result = visit( visitor, operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( expectedErrorCount ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( DataTypeFlag.TypeBool ) );
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
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockAstConditionalBinaryOperatorVisitor();

        var conditionalBinaryOperatorEvaluator = new ConditionalBinaryOperatorEvaluator(
            eventEmitter
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

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( DataTypeFlag.TypeBool ) );
    }
}
