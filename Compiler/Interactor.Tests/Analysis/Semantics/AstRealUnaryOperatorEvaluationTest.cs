using System;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactor.Analysis.Commons.Evaluations.Convolutions.Reals;
using KSPCompiler.Interactor.Analysis.Semantics;

using NUnit.Framework;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

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

        var integerConvolutionEvaluator = new MockIntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new RealConvolutionEvaluator();
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
