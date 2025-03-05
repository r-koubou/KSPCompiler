using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Integers;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.Compilation.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Symbols.MetaData;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class AstIntUnaryOperatorEvaluationTest
{
    [Test]
    public void IntMinusOperatorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockAstUnaryOperatorVisitor();

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var unaryOperatorEvaluator = new NumericUnaryOperatorEvaluator(
            eventEmitter,
            MockUtility.CreateAggregateSymbolTable(),
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( unaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateUnaryMinusOperatorNode( variableName, DataTypeFlag.TypeInt );

        Assert.That( () => visitor.Visit( operatorNode ), Throws.Nothing );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(), Is.EqualTo( 0 ) );
    }

    [Test]
    public void IntNotOperatorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockAstUnaryOperatorVisitor();

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var unaryOperatorEvaluator = new NumericUnaryOperatorEvaluator(
            eventEmitter,
            MockUtility.CreateAggregateSymbolTable(),
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( unaryOperatorEvaluator );

        var operatorNode = MockUtility.CreateUnaryNotOperatorNode( variableName, DataTypeFlag.TypeInt );

        Assert.That( () => visitor.Visit( operatorNode ), Throws.Nothing );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(), Is.EqualTo( 0 ) );
    }
}
