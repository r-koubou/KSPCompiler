using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Booleans;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstBooleanConvolutionEvaluatorTest
{
    public void BooleanConvolutionEvaluatorWithIntegerTest<TNode>( int left, int right, bool expected )
        where TNode : AstExpressionNode, new()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockDefaultAstVisitor();
        var symbols = MockUtility.CreateAggregateSymbolTable();

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator( visitor, symbols.Variables, compilerMessageManger );
        var integerConditionalBinaryOperatorConvolutionCalculator = new IntegerConditionalBinaryOperatorConvolutionCalculator( visitor, integerConvolutionEvaluator );

        var realConvolutionEvaluator = new RealConvolutionEvaluator( visitor, symbols.Variables, compilerMessageManger );
        var realConditionalBinaryOperatorConvolutionCalculator = new RealConditionalBinaryOperatorConvolutionCalculator( visitor, realConvolutionEvaluator );

        var booleanConvolutionEvaluator = new BooleanConvolutionEvaluator(
            visitor,
            integerConditionalBinaryOperatorConvolutionCalculator,
            realConditionalBinaryOperatorConvolutionCalculator
        );

        // 1 == 1
        var ast = new TNode
        {
            TypeFlag = DataTypeFlag.TypeBool,
            Left     = new AstIntLiteralNode( left ),
            Right    = new AstIntLiteralNode( right )
        };

        var result = booleanConvolutionEvaluator.Evaluate( ast, false );

        Assert.IsNotNull( result );
        Assert.AreEqual( expected, result );
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
}

public class BooleanConvolutionEvaluator : IBooleanConvolutionEvaluator
{
    private IAstVisitor Visitor { get; }
    private IIntegerConditionalBinaryOperatorConvolutionCalculator IntegerBinaryOperatorCalculator { get; }
    private IRealConditionalBinaryOperatorConvolutionCalculator RealBinaryOperatorCalculator { get; }
    private IBooleanConditionalUnaryOperatorConvolutionCalculator BooleanBinaryOperatorCalculator { get; }

    public BooleanConvolutionEvaluator(
        IAstVisitor visitor,
        IIntegerConditionalBinaryOperatorConvolutionCalculator integerConvolutionEvaluator,
        IRealConditionalBinaryOperatorConvolutionCalculator realConvolutionEvaluator )
    {
        Visitor = visitor;

        IntegerBinaryOperatorCalculator = integerConvolutionEvaluator;
        RealBinaryOperatorCalculator    = realConvolutionEvaluator;
        BooleanBinaryOperatorCalculator = new BooleanConditionalUnaryOperatorConvolutionCalculator( this );
    }

    public bool? Evaluate( AstExpressionNode expr, bool workingValueForRecursive )
    {
        var id = expr.Id;

        if( expr.Left.Accept( Visitor ) is not AstExpressionNode evaluatedLeft )
        {
            return null;
        }

        if( expr.Right.Accept( Visitor ) is not AstExpressionNode evaluatedRight )
        {
            return null;
        }

        var leftType = evaluatedLeft.TypeFlag;
        var rightType = evaluatedRight.TypeFlag;

        if( leftType != rightType )
        {
            return null;
        }

        if( id.IsBooleanSupportedBinaryOperator() )
        {
            return CalculateBinaryOperator( expr, leftType );
        }

        if( id.IsBooleanSupportedUnaryOperator() )
        {
            throw new NotImplementedException();
        }

        return null;
    }

    private bool? CalculateBinaryOperator( AstExpressionNode expr, DataTypeFlag type )
    {
        if( type.IsInt() )
        {
            return IntegerBinaryOperatorCalculator.Calculate( expr );
        }

        if( type.IsReal() )
        {
            return RealBinaryOperatorCalculator.Calculate( expr );
        }

        if( type.IsBoolean() )
        {
            return BooleanBinaryOperatorCalculator.Calculate( expr );
        }

        return null;
    }
}
