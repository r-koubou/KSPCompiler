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
public class AstIncompatibleBinaryOperatorEvaluationTest
{

    [Test]
    public void BinaryOperationWithIntAndRealWillErrorTest()
    {
        const string variableName = "$x";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstBinaryOperatorVisitor();
        var variableTable = new VariableSymbolTable();
        var variable = MockUtility.CreateIntVariable( variableName );
        variableTable.Add( variable );

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
