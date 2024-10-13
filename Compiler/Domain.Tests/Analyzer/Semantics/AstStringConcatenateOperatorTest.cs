using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;
using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstStringConcatenateOperatorTest
{
    [Test]
    public void StringConcatenateOperator()
    {
        var abortTraverseToken = new AbortTraverseToken();
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstStringConcatenateOperatorVisitor();
        var variableTable = new VariableSymbolTable();

        var integerConvolutionEvaluator = new StringConvolutionEvaluator(
            visitor,
            variableTable,
            compilerMessageManger
        );
        var unaryOperatorEvaluator = new StringConcatenateOperatorEvaluator(
            visitor,
            compilerMessageManger,
            integerConvolutionEvaluator
        );

        visitor.Inject( unaryOperatorEvaluator );

        var operatorNode = new AstStringConcatenateExpressionNode
        {
            Left  = new AstStringLiteralNode( "abc" ),
            Right = new AstStringLiteralNode( "def" )
        };

        var result = visitor.Visit( operatorNode, abortTraverseToken );

        compilerMessageManger.WriteTo( Console.Out );

        var literal = result as AstStringLiteralNode;
        Assert.IsTrue( literal is not null );
        Assert.IsTrue( literal?.Value == "abcdef" );

        Assert.IsFalse( abortTraverseToken.Aborted );
        Assert.IsFalse( compilerMessageManger.Count() > 0 );
    }
}
