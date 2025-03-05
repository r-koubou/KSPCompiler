using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Integers;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.Compilation.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Symbols.MetaData;
using KSPCompiler.Shared.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class AstIncompatibleBinaryOperatorEvaluationTest
{

    [Test]
    public void BinaryOperationWithIntAndRealWillErrorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockAstBinaryOperatorVisitor();

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            eventEmitter,
            MockUtility.CreateAggregateSymbolTable(),
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var right = new AstRealLiteralNode( 1.0 );
        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstAdditionExpressionNode>( variableName, DataTypeUtility.GuessFromSymbolName( variableName ), right );
        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(), Is.GreaterThan( 0 ) );
        Assert.That( result, Is.Not.Null );
        Assert.That(result?.TypeFlag.IsInt(),  Is.True);
        Assert.That(result?.TypeFlag.IsReal(), Is.True);
    }
}
