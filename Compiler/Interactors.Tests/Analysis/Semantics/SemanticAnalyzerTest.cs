using System;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.UseCases.Analysis.Context;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class SemanticAnalyzerTest
{
    private static IAnalyzerContext CreateContext()
        => new SemanticAnalyzerContext(
            ICompilerMessageManger.Default,
            MockUtility.CreateAggregateSymbolTable()
        );

    [Test]
    public void AssignmentRightNodeWillBeReplacedIntLiteralNode()
    {
        // $x := 1 + 2 + 3
        // => $x := 6  { replaced via convolution }

        var context = CreateContext();
        var semanticAnalyzer = new SemanticAnalyzer( context );
        var variableSymbol = MockUtility.CreateIntVariable( "$x" );
        var variable = MockUtility.CreateSymbolNode( "$x", DataTypeFlag.TypeInt );

        context.SymbolTable.Variables.Add( variableSymbol );

        var statement = new AstAssignmentExpressionNode(
            variable,
            new AstAdditionExpressionNode(
                new AstAdditionExpressionNode(
                    new AstIntLiteralNode( 1 ),
                    new AstIntLiteralNode( 2 )
                ),
                new AstIntLiteralNode( 3 )
            )
        );

        context.ExpressionContext.AssignOperator.Evaluate( semanticAnalyzer, statement );
        context.CompilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0,    context.CompilerMessageManger.Count() );
        Assert.AreEqual( true, statement.Right is AstIntLiteralNode );
        Assert.AreEqual( 6,    ( statement.Right as AstIntLiteralNode )?.Value );
    }

    [Test]
    public void AssignmentRightNodeWillBeReplacedRealLiteralNode()
    {
        // ~x := 1.0 + 2.0 + 3.0
        // => ~x := 6.0  { replaced via convolution }

        var context = CreateContext();
        var semanticAnalyzer = new SemanticAnalyzer( context );
        var variableSymbol = MockUtility.CreateRealVariable( "~x" );
        var variable = MockUtility.CreateSymbolNode( "~x", DataTypeFlag.TypeReal );

        context.SymbolTable.Variables.Add( variableSymbol );

        var statement = new AstAssignmentExpressionNode(
            variable,
            new AstAdditionExpressionNode(
                new AstAdditionExpressionNode(
                    new AstRealLiteralNode( 1.0 ),
                    new AstRealLiteralNode( 2.0 )
                ),
                new AstRealLiteralNode( 3.0 )
            )
        );

        context.ExpressionContext.AssignOperator.Evaluate( semanticAnalyzer, statement );
        context.CompilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0,    context.CompilerMessageManger.Count() );
        Assert.AreEqual( true, statement.Right is AstRealLiteralNode );
        Assert.AreEqual( 6,    ( statement.Right as AstRealLiteralNode )?.Value );
    }

    [Test]
    public void AssignmentRightNodeWillBeReplacedStringLiteralNode()
    {
        // @x := "abc" & 123 & "def"
        // => @x := "abc123def"  { replaced via convolution }

        var context = CreateContext();
        var semanticAnalyzer = new SemanticAnalyzer( context );
        var variableSymbol = MockUtility.CreateStringVariable( "@x" );
        var variable = MockUtility.CreateSymbolNode( "@x", DataTypeFlag.TypeString );

        context.SymbolTable.Variables.Add( variableSymbol );

        var statement = new AstAssignmentExpressionNode(
            variable,
            new AstStringConcatenateExpressionNode(
                new AstStringConcatenateExpressionNode(
                    new AstStringLiteralNode( "abc" ),
                    new AstIntLiteralNode( 123 )
                ),
                new AstStringLiteralNode( "def" )
            )
        );

        context.ExpressionContext.AssignOperator.Evaluate( semanticAnalyzer, statement );

        context.CompilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0,           context.CompilerMessageManger.Count() );
        Assert.AreEqual( true,        statement.Right is AstStringLiteralNode );
        Assert.AreEqual( "abc123def", ( statement.Right as AstStringLiteralNode )?.Value );
    }
}
