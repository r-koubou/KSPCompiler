using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstRealBinaryOperatorEvaluationTest
{
    #region Mathmetical Operators

    [Test]
    public void AddOperatorTest()
    {
        const string variableName = "~x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstAdditionExpressionNode>( variableName, DataTypeFlag.TypeReal, new AstRealLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsReal() );
    }

    [Test]
    public void SubOperatorTest()
    {
        const string variableName = "~x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstSubtractionExpressionNode>( variableName, DataTypeFlag.TypeReal, new AstRealLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsReal() );
    }

    [Test]
    public void MulOperatorTest()
    {
        const string variableName = "~x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstMultiplyingExpressionNode>( variableName, DataTypeFlag.TypeReal, new AstRealLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsReal() );
    }

    [Test]
    public void DivOperatorTest()
    {
        const string variableName = "~x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstDivisionExpressionNode>( variableName, DataTypeFlag.TypeReal, new AstRealLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsReal() );
    }

    #endregion ~Mathmetical Operators

    #region Not Supported Operators

    [Test]
    public void CannotModOperatorTest()
    {
        const string variableName = "~x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstModuloExpressionNode>( variableName, DataTypeFlag.TypeReal, new AstRealLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsReal() );
    }

    [Test]
    public void CannotBitwiseOrOperatorTest()
    {
        const string variableName = "~x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstBitwiseOrExpressionNode>( variableName, DataTypeFlag.TypeReal, new AstRealLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsReal() );
    }

    [Test]
    public void CannotBitwiseAndOperatorTest()
    {
        const string variableName = "~x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstBitwiseAndExpressionNode>( variableName, DataTypeFlag.TypeReal, new AstRealLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsReal() );
    }

    [Test]
    public void CannotBitwiseXorOperatorTest()
    {
        const string variableName = "~x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstBitwiseXorExpressionNode>( variableName, DataTypeFlag.TypeReal, new AstRealLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsReal() );
    }

    #endregion ~Not Supported Operators


}
