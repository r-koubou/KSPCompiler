using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Context;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.Compilation.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class SemanticAnalyzerTest
{
    private static IAnalyzerContext CreateContext()
        => new SemanticAnalyzerContext(
            new MockEventEmitter(),
            new AggregateSymbolTable()
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

        var compilerMessageManger = ICompilerMessageManger.Default;
        context.EventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        context.SymbolTable.UserVariables.Add( variableSymbol );

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
        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(),           Is.EqualTo( 0 ) );
        Assert.That( statement.Right,                                 Is.InstanceOf<AstIntLiteralNode>() );
        Assert.That( ( statement.Right as AstIntLiteralNode )?.Value, Is.EqualTo( 6 ) );
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

        var compilerMessageManger = ICompilerMessageManger.Default;
        context.EventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        context.SymbolTable.UserVariables.Add( variableSymbol );

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
        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(),            Is.EqualTo( 0 ) );
        Assert.That( statement.Right,                                  Is.InstanceOf<AstRealLiteralNode>() );
        Assert.That( ( statement.Right as AstRealLiteralNode )?.Value, Is.EqualTo( 6 ) );
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

        var compilerMessageManger = ICompilerMessageManger.Default;
        context.EventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        context.SymbolTable.UserVariables.Add( variableSymbol );

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

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(),              Is.EqualTo( 0 ) );
        Assert.That( statement.Right,                                    Is.InstanceOf<AstStringLiteralNode>() );
        Assert.That( ( statement.Right as AstStringLiteralNode )?.Value, Is.EqualTo( "abc123def" ) );
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
                DataType = DataTypeFlag.All
            }
        );

        var variable = MockUtility.CreateSymbolNode( "@x", DataTypeFlag.TypeString );

        var compilerMessageManger = ICompilerMessageManger.Default;
        context.EventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

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
        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(), Is.EqualTo( 0 ) );

        var args = expr.Right as AstExpressionListNode;

        Assert.That( args,                                                   Is.Not.Null );
        Assert.That( args?.Expressions[ 0 ],                                 Is.InstanceOf<AstIntLiteralNode>() );
        Assert.That( ( args?.Expressions[ 0 ] as AstIntLiteralNode )?.Value, Is.EqualTo( 6 ) );
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

        var compilerMessageManger = ICompilerMessageManger.Default;
        context.EventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        context.DeclarationContext.Variable.Evaluate( semanticAnalyzer, statement );
        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(),                                                 Is.EqualTo( 0 ) );
        Assert.That( statement.Initializer.PrimitiveInitializer.Expression,                                 Is.InstanceOf<AstIntLiteralNode>() );
        Assert.That( ( statement.Initializer.PrimitiveInitializer.Expression as AstIntLiteralNode )?.Value, Is.EqualTo( 6 ) );
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

        var compilerMessageManger = ICompilerMessageManger.Default;
        context.EventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        context.ExpressionContext.ConditionalBinaryOperator.Evaluate( semanticAnalyzer, expr );
        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count(),      Is.EqualTo( 0 ) );
        Assert.That( expr.Left,                                  Is.InstanceOf<AstIntLiteralNode>() );
        Assert.That( ( expr.Left as AstIntLiteralNode )?.Value,  Is.EqualTo( 6 ) );
        Assert.That( expr.Right,                                 Is.InstanceOf<AstIntLiteralNode>() );
        Assert.That( ( expr.Right as AstIntLiteralNode )?.Value, Is.EqualTo( 6 ) );
    }
}
