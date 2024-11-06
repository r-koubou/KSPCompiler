using System;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactor.Analysis.Commons.Evaluations.Convolutions.Integers;
using KSPCompiler.Interactor.Analysis.Semantics;

using NUnit.Framework;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

[TestFixture]
public class AstIntUnaryOperatorEvaluationTest
{
    [Test]
    public void IntMinusOperatorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstUnaryOperatorVisitor();

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var unaryOperatorEvaluator = new NumericUnaryOperatorEvaluator(
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( unaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateUnaryMinusOperatorNode( variableName, DataTypeFlag.TypeInt );

        Assert.DoesNotThrow( () => visitor.Visit( operatorNode ) );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
    }

    [Test]
    public void IntNotOperatorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstUnaryOperatorVisitor();

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var unaryOperatorEvaluator = new NumericUnaryOperatorEvaluator(
            compilerMessageManger,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( unaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateUnaryNotOperatorNode( variableName, DataTypeFlag.TypeInt );

        Assert.DoesNotThrow( () => visitor.Visit( operatorNode ) );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count() > 0 );
    }
}
