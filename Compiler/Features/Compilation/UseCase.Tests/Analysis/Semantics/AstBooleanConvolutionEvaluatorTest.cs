using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class AstBooleanConvolutionEvaluatorTest
{
    public void BooleanConvolutionEvaluatorWithIntegerTest<TNode>( int left, int right, bool expected )
        where TNode : AstExpressionNode, new()
    {
        var visitor = new MockDefaultAstVisitor();
        var booleanConvolutionEvaluator = MockUtility.CreateBooleanConvolutionEvaluator( visitor );

        var ast = new TNode
        {
            TypeFlag = DataTypeFlag.TypeBool,
            Left     = new AstIntLiteralNode( left ),
            Right    = new AstIntLiteralNode( right )
        };

        var result = booleanConvolutionEvaluator.Evaluate( visitor, ast, false );

        Assert.That( result, Is.Not.Null );
        Assert.That( result, Is.EqualTo( expected ) );
    }

    [TestCase( 1, 1, true )]
    [TestCase( 1, 2, false )]
    public void BooleanConvolutionEvaluatorWithIntegerEqualTest( int left, int right, bool expected )
        // left == right
        => BooleanConvolutionEvaluatorWithIntegerTest<AstEqualExpressionNode>( left, right, expected );

    [TestCase( 1, 1, false )]
    [TestCase( 1, 2, true )]
    public void BooleanConvolutionEvaluatorWithIntegerNotEqualTest( int left, int right, bool expected )
        // left != right
        => BooleanConvolutionEvaluatorWithIntegerTest<AstNotEqualExpressionNode>( left, right, expected );

    [TestCase( 1, 1, false )]
    [TestCase( 1, 2, true )]
    [TestCase( 2, 2, false )]
    public void BooleanConvolutionEvaluatorWithIntegerLessThanTest( int left, int right, bool expected )
        // left < right
        => BooleanConvolutionEvaluatorWithIntegerTest<AstLessThanExpressionNode>( left, right, expected );

    [TestCase( 1, 1, false )]
    [TestCase( 1, 2, false )]
    [TestCase( 2, 1, true )]
    public void BooleanConvolutionEvaluatorWithIntegerGreaterThanTest( int left, int right, bool expected )
        // left > right
        => BooleanConvolutionEvaluatorWithIntegerTest<AstGreaterThanExpressionNode>( left, right, expected );

    [TestCase( 1, 1, true )]
    [TestCase( 1, 2, true )]
    [TestCase( 2, 1, false )]
    public void BooleanConvolutionEvaluatorWithIntegerLessEqualTest( int left, int right, bool expected )
        // left <= right
        => BooleanConvolutionEvaluatorWithIntegerTest<AstLessEqualExpressionNode>( left, right, expected );

    [TestCase( 1, 1, true )]
    [TestCase( 1, 2, false )]
    [TestCase( 2, 1, true )]
    public void BooleanConvolutionEvaluatorWithIntegerGreaterEqualTest( int left, int right, bool expected )
        // left >= right
        => BooleanConvolutionEvaluatorWithIntegerTest<AstGreaterEqualExpressionNode>( left, right, expected );

    [TestCase( true )]
    [TestCase( false )]
    public void BooleanConvolutionEvaluatorWithIntegerUnaryLogicalNotTest( bool value )
    {
        var visitor = new MockDefaultAstVisitor();
        var booleanConvolutionEvaluator = MockUtility.CreateBooleanConvolutionEvaluator( visitor );

        var ast = new AstUnaryLogicalNotExpressionNode
        {
            TypeFlag = DataTypeFlag.TypeBool,
            Left     = new AstBooleanLiteralNode( value )
        };

        var result = booleanConvolutionEvaluator.Evaluate( visitor, ast, false );

        Assert.That( result, Is.Not.Null );
        Assert.That( result, Is.EqualTo( !value ) );
    }

    [TestCase( 1, 1, 1, 1, true )]
    [TestCase( 1, 1, 2, 1, false )]
    public void LogicalAndConditionalOperatorTest(int left1, int right1, int left2, int right2, bool expected )
    {
        var visitor = new MockDefaultAstVisitor();
        var booleanConvolutionEvaluator = MockUtility.CreateBooleanConvolutionEvaluator( visitor );

        var ast = new AstLogicalAndExpressionNode
        {
            TypeFlag = DataTypeFlag.TypeBool,
            Left = new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( left1 ),
                Right    = new AstIntLiteralNode( right1 ),
                Constant = true
            },
            Right = new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( left2 ),
                Right    = new AstIntLiteralNode( right2 ),
                Constant = true
            }
        };

        var result = booleanConvolutionEvaluator.Evaluate( visitor, ast, false );

        Assert.That( result, Is.Not.Null );
        Assert.That( result, Is.EqualTo( expected ) );
    }

    [TestCase( 1, 1, 1, 1, true )]
    [TestCase( 2, 1, 2, 1, false )]
    public void LogicalOrConditionalOperatorTest(int left1, int right1, int left2, int right2, bool expected )
    {
        var visitor = new MockDefaultAstVisitor();
        var booleanConvolutionEvaluator = MockUtility.CreateBooleanConvolutionEvaluator( visitor );

        var ast = new AstLogicalOrExpressionNode
        {
            TypeFlag = DataTypeFlag.TypeBool,
            Left = new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( left1 ),
                Right    = new AstIntLiteralNode( right1 ),
                Constant = true
            },
            Right = new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( left2 ),
                Right    = new AstIntLiteralNode( right2 ),
                Constant = true
            }
        };

        var result = booleanConvolutionEvaluator.Evaluate( visitor, ast, false );

        Assert.That( result, Is.Not.Null );
        Assert.That( result, Is.EqualTo( expected ) );
    }

    [TestCase( 1, 1, 1, 1, false )]
    [TestCase( 1, 2, 1, 2, false )]
    [TestCase( 1, 1, 1, 2, true )]
    [TestCase( 1, 2, 1, 1, true )]
    public void LogicalXorConditionalOperatorTest( int left1, int right1, int left2, int right2, bool expected )
    {
        var visitor = new MockDefaultAstVisitor();
        var booleanConvolutionEvaluator = MockUtility.CreateBooleanConvolutionEvaluator( visitor );

        var ast = new AstLogicalXorExpressionNode
        {
            TypeFlag = DataTypeFlag.TypeBool,
            Left = new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( left1 ),
                Right    = new AstIntLiteralNode( right1 ),
                Constant = true
            },
            Right = new AstEqualExpressionNode
            {
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( left2 ),
                Right    = new AstIntLiteralNode( right2 ),
                Constant = true
            }
        };

        var result = booleanConvolutionEvaluator.Evaluate( visitor, ast, false );

        Assert.That( result, Is.Not.Null );
        Assert.That( result, Is.EqualTo( expected ) );
    }
}
