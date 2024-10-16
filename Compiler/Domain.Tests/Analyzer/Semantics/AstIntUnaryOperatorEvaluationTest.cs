using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstIntUnaryOperatorEvaluationTest
{
    [Test]
    public void IntMinusOperatorTest()
    {
        const string variableName = "$x";

        var abortTraverseToken = new AbortTraverseToken();
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstUnaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var unaryOperatorEvaluator = new NumericUnaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( unaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateUnaryMinusOperatorNode( variableName, DataTypeFlag.TypeInt );

        Assert.DoesNotThrow( () => visitor.Visit( operatorNode, abortTraverseToken ) );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( abortTraverseToken.Aborted );
        Assert.IsFalse( compilerMessageManger.Count() > 0 );
    }

    [Test]
    public void IntNotOperatorTest()
    {
        const string variableName = "$x";

        var abortTraverseToken = new AbortTraverseToken();
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstUnaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var unaryOperatorEvaluator = new NumericUnaryOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( unaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateUnaryNotOperatorNode( variableName, DataTypeFlag.TypeInt );

        Assert.DoesNotThrow( () => visitor.Visit( operatorNode, abortTraverseToken ) );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( abortTraverseToken.Aborted );
        Assert.IsFalse( compilerMessageManger.Count() > 0 );
    }
}
