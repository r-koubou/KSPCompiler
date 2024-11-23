using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Integers;
using KSPCompiler.Interactors.Analysis.Semantics;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstIntBinaryOperatorConvolutionTest
{
    private void ConvolutionTestBody( int expected, Func<IAstVisitor, AstIntLiteralNode?> visitImpl )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            compilerMessageManger,
            MockUtility.CreateAggregateSymbolTable().Variables,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var result = visitImpl( visitor );

        compilerMessageManger.WriteTo( Console.Out );

        ClassicAssert.AreEqual( 0, compilerMessageManger.Count() );
        ClassicAssert.IsNotNull( result );
        ClassicAssert.AreEqual( expected, result?.Value );
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
