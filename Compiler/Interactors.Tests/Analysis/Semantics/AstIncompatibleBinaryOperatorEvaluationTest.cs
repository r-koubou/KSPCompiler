using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations.Convolutions.Integers;
using KSPCompiler.Interactors.Analysis.Semantics;

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
        var visitor = new MockAstBinaryOperatorVisitor();

        var integerConvolutionEvaluator = new IntegerConvolutionEvaluator();
        var realConvolutionEvaluator = new MockRealConvolutionEvaluator();
        var binaryOperatorEvaluator = new NumericBinaryOperatorEvaluator(
            compilerMessageManger,
            MockUtility.CreateAggregateSymbolTable().Variables,
            integerConvolutionEvaluator,
            realConvolutionEvaluator
        );

        visitor.Inject( binaryOperatorEvaluator );

        var right = new AstRealLiteralNode( 1.0 );
        var operatorNode = MockUtility.CreateBinaryOperatorNode<AstAdditionExpressionNode>( variableName, DataTypeUtility.GuessFromSymbolName( variableName ), right );
        var result = visitor.Visit( operatorNode ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( compilerMessageManger.Count() > 0 );
        Assert.IsNotNull( result );
        Assert.IsTrue( result?.TypeFlag.IsInt() );
        Assert.IsTrue( result?.TypeFlag.IsReal() );
    }
}
