using System;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Reals;
using KSPCompiler.Interactors.Analysis.Semantics;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstRealUnaryOperatorEvaluationTest
{
    [Test]
    public void RealMinusOperatorTest()
    {
        const string variableName = "~x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstUnaryOperatorVisitor();

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator();
        var unaryOperatorEvaluator = new NumericUnaryOperatorEvaluator(
            compilerMessageManger,
            MockUtility.CreateAggregateSymbolTable().Variables,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( unaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateUnaryMinusOperatorNode( variableName, DataTypeFlag.TypeReal );

        Assert.That( () => visitor.Visit( operatorNode ), Throws.Nothing );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(), Is.EqualTo( 0 ) );
    }

    [Test]
    public void CannotRealNotOperatorTest()
    {
        const string variableName = "~x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstUnaryOperatorVisitor();

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator();
        var unaryOperatorEvaluator = new NumericUnaryOperatorEvaluator(
            compilerMessageManger,
            MockUtility.CreateAggregateSymbolTable().Variables,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( unaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateUnaryNotOperatorNode( variableName, DataTypeFlag.TypeReal );

        Assert.That( () => visitor.Visit( operatorNode ), Throws.Nothing );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(), Is.GreaterThan( 0 ) );
    }

}
