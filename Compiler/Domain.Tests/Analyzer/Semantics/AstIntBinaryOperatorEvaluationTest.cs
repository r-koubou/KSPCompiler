using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
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
public class AstIntBinaryOperatorEvaluationTest
{

    #region Mathmatical Operators

    [Test]
    public void IntAddOperatorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstAdditionExpressionNode>( variableName, DataTypeFlag.TypeInt, new AstIntLiteralNode( 1 ) );
        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsInt() );
    }

    [Test]
    public void IntSubOperatorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstSubtractionExpressionNode>( variableName, DataTypeFlag.TypeInt, new AstIntLiteralNode( 1 ) );
        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsInt() );
    }

    [Test]
    public void IntMulOperatorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstMultiplyingExpressionNode>( variableName, DataTypeFlag.TypeInt, new AstIntLiteralNode( 1 ) );
        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsInt() );
    }

    [Test]
    public void IntDivOperatorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstDivisionExpressionNode>( variableName, DataTypeFlag.TypeInt, new AstIntLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsInt() );
    }

    [Test]
    public void IntModOperatorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstModuloExpressionNode>( variableName, DataTypeFlag.TypeInt, new AstIntLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsInt() );
    }

    #endregion ~Mathmatical Operators

    [Test]
    public void BitwiseOrOperatorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstBitwiseOrExpressionNode>( variableName, DataTypeFlag.TypeInt, new AstIntLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsInt() );
    }

    [Test]
    public void BitwiseAndOperatorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstBitwiseAndExpressionNode>( variableName, DataTypeFlag.TypeInt, new AstIntLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsInt() );
    }

    [Test]
    public void BitwiseXorOperatorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstBitwiseXorExpressionNode>( variableName, DataTypeFlag.TypeInt, new AstIntLiteralNode( 1 ) );

        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsInt() );
    }
}
