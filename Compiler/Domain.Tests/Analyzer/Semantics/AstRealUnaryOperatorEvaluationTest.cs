using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstRealUnaryOperatorEvaluationTest
{
    [Test]
    public void RealMinusOperatorTest()
    {
        const string variableName = "~x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstUnaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var unaryOperatorEvaluator = new NumericUnaryOperatorEvaluator(
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( unaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateUnaryMinusOperatorNode( variableName, DataTypeFlag.TypeReal );

        Assert.DoesNotThrow( () => visitor.Visit( operatorNode ) );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
    }

    [Test]
    public void CannotRealNotOperatorTest()
    {
        const string variableName = "~x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstUnaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        variableTable.Add( MockUtility.CreateIntVariable( variableName ) );

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var unaryOperatorEvaluator = new NumericUnaryOperatorEvaluator(
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( unaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateUnaryNotOperatorNode( variableName, DataTypeFlag.TypeReal );

        Assert.DoesNotThrow( () => visitor.Visit( operatorNode ) );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( compilerMessageManger.Count() > 0 );
    }

}
