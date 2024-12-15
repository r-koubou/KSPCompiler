using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Integers;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.Interactors.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

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
            MockUtility.CreateAggregateSymbolTable().UserVariables,
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
