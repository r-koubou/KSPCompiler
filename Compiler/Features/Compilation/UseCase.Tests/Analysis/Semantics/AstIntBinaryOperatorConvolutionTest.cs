using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Integers;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.Compilation.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class AstIntBinaryOperatorConvolutionTest
{
    private void ConvolutionTestBody( int expected, Func<IAstVisitor, AstIntLiteralNode?> visitImpl )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockAstBinaryOperatorVisitor();

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            eventEmitter,
            MockUtility.CreateAggregateSymbolTable(),
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var result = visitImpl( visitor );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(), Is.EqualTo( 0 ) );
        Assert.That( result,                        Is.Not.Null );
        Assert.That( result?.Value,                 Is.EqualTo( expected ) );
    }

    [TestCase( 1, 1,  2 )]
    [TestCase( 1, 2,  3 )]
    [TestCase( 2, -1, 1 )]
    public void IntAddConvolutionTest( int left, int right, int expected )
        => ConvolutionTestBody( expected, visitor =>
        {
            var operatorNode = new AstAdditionExpressionNode
            {
                Left  = new AstIntLiteralNode( left ),
                Right = new AstIntLiteralNode( right )
            };

            return visitor.Visit( operatorNode ) as AstIntLiteralNode;
        });

    [TestCase( 1, 1,  0 )]
    [TestCase( 1, 2,  -1 )]
    [TestCase( 2, -1, 3 )]
    public void IntSubConvolutionTest( int left, int right, int expected )
        => ConvolutionTestBody( expected, visitor =>
        {
            var operatorNode = new AstSubtractionExpressionNode
            {
                Left  = new AstIntLiteralNode( left ),
                Right = new AstIntLiteralNode( right )
            };

            return visitor.Visit( operatorNode ) as AstIntLiteralNode;
        });

    [TestCase( 1, 1,  1 )]
    [TestCase( 1, 2,  2 )]
    [TestCase( 2, -1, -2 )]
    public void IntMulConvolutionTest( int left, int right, int expected )
        => ConvolutionTestBody( expected, visitor =>
        {
            var operatorNode = new AstMultiplyingExpressionNode
            {
                Left  = new AstIntLiteralNode( left ),
                Right = new AstIntLiteralNode( right )
            };

            return visitor.Visit( operatorNode ) as AstIntLiteralNode;
        });

    [TestCase( 4, 2,  2 )]
    [TestCase( 6, 3,  2 )]
    [TestCase( -4, 2, -2 )]
    public void IntDivConvolutionTest( int left, int right, int expected )
        => ConvolutionTestBody( expected, visitor =>
        {
            var operatorNode = new AstDivisionExpressionNode
            {
                Left  = new AstIntLiteralNode( left ),
                Right = new AstIntLiteralNode( right )
            };

            return visitor.Visit( operatorNode ) as AstIntLiteralNode;
        });

    [TestCase( 4,  2, 0 )]
    [TestCase( 6,  4, 2 )]
    [TestCase( -4, 3, -1 )]
    public void IntModConvolutionTest( int left, int right, int expected )
        => ConvolutionTestBody( expected, visitor =>
        {
            var operatorNode = new AstModuloExpressionNode
            {
                Left  = new AstIntLiteralNode( left ),
                Right = new AstIntLiteralNode( right )
            };

            return visitor.Visit( operatorNode ) as AstIntLiteralNode;
        });

    [TestCase( 1,  2, 3 )]
    [TestCase( 3,  5, 7 )]
    [TestCase( 11, 0, 11 )]
    public void IntBitwiseOrConvolutionTest( int left, int right, int expected )
        => ConvolutionTestBody( expected, visitor =>
        {
            var operatorNode = new AstBitwiseOrExpressionNode
            {
                Left  = new AstIntLiteralNode( left ),
                Right = new AstIntLiteralNode( right )
            };

            return visitor.Visit( operatorNode ) as AstIntLiteralNode;
        });

    [TestCase( 1,  2, 0 )]
    [TestCase( 3,  5, 1 )]
    [TestCase( 11, 15, 11 )]
    public void IntBitwiseAndConvolutionTest( int left, int right, int expected )
        => ConvolutionTestBody( expected, visitor =>
        {
            var operatorNode = new AstBitwiseAndExpressionNode
            {
                Left  = new AstIntLiteralNode( left ),
                Right = new AstIntLiteralNode( right )
            };

            return visitor.Visit( operatorNode ) as AstIntLiteralNode;
        });

    [TestCase( 1,  2, 3 )]
    [TestCase( 3,  5,  6 )]
    [TestCase( 11, 15, 4 )]
    public void IntBitwiseXorConvolutionTest( int left, int right, int expected )
        => ConvolutionTestBody( expected, visitor =>
        {
            var operatorNode = new AstBitwiseXorExpressionNode
            {
                Left  = new AstIntLiteralNode( left ),
                Right = new AstIntLiteralNode( right )
            };

            return visitor.Visit( operatorNode ) as AstIntLiteralNode;
        });
}
