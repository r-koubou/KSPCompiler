using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
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

    [Test]
    public void CommandArgumentNodeWillBeReplacedStringLiteralNode()
    {
        // message( 1 + 2 + 3 )
        // => message( 6 )  { replaced via convolution }

        var context = CreateContext();
        var semanticAnalyzer = new SemanticAnalyzer( context );
        var command = MockUtility.CreateCommand(
            "message",
            DataTypeFlag.TypeVoid,
            new CommandArgumentSymbol
            {
                DataType = DataTypeFlag.MultipleType
            }
        );

        var variable = MockUtility.CreateSymbolNode( "@x", DataTypeFlag.TypeString );

        context.SymbolTable.Commands.Add( command );

        // message( 1 + 2 + 3 )
        var expr = MockUtility.CreateCommandExpressionNode(
            "message",
            new AstAdditionExpressionNode(
                new AstAdditionExpressionNode(
                    new AstIntLiteralNode( 1 ),
                    new AstIntLiteralNode( 2 )
                ),
                new AstIntLiteralNode( 3 )
            )
        );

        context.ExpressionContext.CallCommand.Evaluate( semanticAnalyzer, expr );
        context.CompilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0, context.CompilerMessageManger.Count() );

        var args = expr.Right as AstExpressionListNode;

        Assert.IsNotNull( args );
        Assert.AreEqual( true, args?.Expressions[0] is AstIntLiteralNode );
        Assert.AreEqual( 6,    ( args?.Expressions[ 0 ] as AstIntLiteralNode )?.Value );
    }

    [Test]
    public void DeclarationWillBeReplacedIntLiteralNode()
    {
        // on init
        //     declare $x := 1 + 2 + 3
        //      => declare $x := 6  { replaced via convolution }
        // end on


        var context = CreateContext();
        var semanticAnalyzer = new SemanticAnalyzer( context );

        var init = new AstCallbackDeclarationNode
        {
            Name = "init"
        };

        var statement = new AstVariableDeclarationNode
        {
            Name = "$x",
        };

        var initializer = new AstAdditionExpressionNode(
            statement,
            new AstAdditionExpressionNode(
                new AstIntLiteralNode( 1 ),
                new AstIntLiteralNode( 2 )
            ),
            new AstIntLiteralNode( 3 )
        );

        statement.Initializer = new AstVariableInitializerNode
        {
            PrimitiveInitializer = new AstPrimitiveInitializerNode
            {
                Expression = initializer
            }
        };

        statement.Parent = init;
        init.Block.Statements.Add( statement );

        context.DeclarationContext.Variable.Evaluate( semanticAnalyzer, statement );
        context.CompilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0,    context.CompilerMessageManger.Count() );
        Assert.AreEqual( true, statement.Initializer.PrimitiveInitializer.Expression is AstIntLiteralNode );
        Assert.AreEqual( 6,    ( statement.Initializer.PrimitiveInitializer.Expression as AstIntLiteralNode )?.Value );
    }

    [Test]
    public void ConditionalBinaryOperatorNodeWillBeReplacedIntLiteralNode()
    {
        // 1 + 2 + 3 = 1 + 2 + 3
        // => 6 = 6  { replaced via convolution }

        var context = CreateContext();
        var semanticAnalyzer = new SemanticAnalyzer( context );

        var expr = new AstEqualExpressionNode{
            Left = new AstAdditionExpressionNode(
                new AstAdditionExpressionNode(
                    new AstIntLiteralNode( 1 ),
                    new AstIntLiteralNode( 2 )
                ),
                new AstIntLiteralNode( 3 )
            ),
            Right = new AstAdditionExpressionNode(
                new AstAdditionExpressionNode(
                    new AstIntLiteralNode( 1 ),
                    new AstIntLiteralNode( 2 )
                ),
                new AstIntLiteralNode( 3 )
            )
        };

        context.ExpressionContext.ConditionalBinaryOperator.Evaluate( semanticAnalyzer, expr );
        context.CompilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0,    context.CompilerMessageManger.Count() );
        Assert.AreEqual( true, expr.Left is AstIntLiteralNode );
        Assert.AreEqual( 6,    ( expr.Left as AstIntLiteralNode )?.Value );
        Assert.AreEqual( true, expr.Right is AstIntLiteralNode );
        Assert.AreEqual( 6,    ( expr.Right as AstIntLiteralNode )?.Value );
    }
}
